using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
	/// <summary>
	/// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
	/// </summary>
	[Serializable]
	public abstract class Shape
	{
		#region Constructors
		
		public Shape()
		{
		}
		
		public Shape(RectangleF rect)
		{
			rectangle = rect;
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.Location = shape.Location;
			this.rectangle = shape.rectangle;
			
			this.FillColor =  shape.FillColor;
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Обхващащ правоъгълник на елемента.
		/// </summary>
		private RectangleF rectangle;		
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}

		private string shapeName;
		public string ShapeName {
			get { return shapeName; }
			set { shapeName = value; }
		}
		/// <summary>
		/// Широчина на елемента.
		/// </summary>
		/// 
		public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		
		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location {
			get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}
		private Color borderColor;
		public Color BorderColor{
			get { return borderColor; }
			set { borderColor = value; }
		}
		/// <summary>
		/// Цвят на елемента.
		/// </summary>
		private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}
		private int opacity;
		public int Opacity {
			get{ return opacity; }
			set { opacity = value; }
		}
		private int lineWidth;
		public virtual int  LineWidth 
		{
			get { return lineWidth; }
			set { lineWidth = value; }
		}

		private float shapeAngle;
		public float ShapeAngle {
			get { return shapeAngle; }
			set { shapeAngle = value; }
		}

		#endregion


		/// <summary>
		/// Проверка дали точка point принадлежи на елемента.
		/// </summary>
		/// <param name="point">Точка</param>
		/// <returns>Връща true, ако точката принадлежи на елемента и
		/// false, ако не пренадлежи</returns>
		public virtual bool Contains(PointF point)
		{
			return Rectangle.Contains(point.X, point.Y);
		}

		/// <summary>
		/// Визуализира елемента.
		/// </summary>
		/// <param name="grfx">Къде да бъде визуализиран елемента.</param>
		public virtual void DrawSelf(Graphics grfx)
		{
			//shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
		}

		public virtual void Rotate(Graphics g)
		{
			double a = Rectangle.Width / 2;
			double b = Rectangle.Height / 2;

			double RectX = Rectangle.Location.X + a;
			double RectY = Rectangle.Location.Y + b;
			Point center = new Point((int)RectX, (int)RectY);

			Matrix myMatrix = new Matrix();
			myMatrix.RotateAt(shapeAngle, center);
			g.Transform = myMatrix;
		}

		public virtual void Move(PointF p,PointF lastLocation)
		{
			Location = new PointF(Location.X + p.X - lastLocation.X, Location.Y + p.Y - lastLocation.Y);
		}
		public virtual object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
