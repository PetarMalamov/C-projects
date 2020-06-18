using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Draw.src.Model;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection
		{
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		private Color fillColor;
		public Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}
		private int opacity;
		public int Opacity {
			get { return opacity; }
			set { opacity = value; }
		}
		public List<Shape> shapeCopies = new List<Shape>();
		public List<Shape> ShapeCopies {
			get { return shapeCopies; }
			set { shapeCopies = value; }
		}
		#endregion

		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = fillColor;
			rect.Opacity = opacity;
			rect.BorderColor = Color.Black;
			ShapeList.Add(rect);
		}

		public void AddRandomTriangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			//RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
			TriangleShape tri = new TriangleShape(new Rectangle(x, y, 100, 200));
			tri.FillColor = fillColor;
			tri.Opacity = opacity;
			tri.BorderColor = Color.Black;

			ShapeList.Add(tri);
		}
		public void AddRandomElipse()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			ElipseShape ellipse = new ElipseShape(new Rectangle(x, y, 100, 200));
			ellipse.FillColor = fillColor;
			ellipse.Opacity = opacity;
			ellipse.BorderColor = Color.Black;

			ShapeList.Add(ellipse);
		}

		public void AddRandomEighth()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			EighthShape eighth = new EighthShape(new Rectangle(x, y, 100, 200));
			eighth.FillColor = fillColor;
			eighth.Opacity = opacity;
			eighth.BorderColor = Color.Black;

			ShapeList.Add(eighth);
		}
		public void AddRandomLetter()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			LetterShape let = new LetterShape(new Rectangle(x, y, 300, 200));
			let.FillColor = fillColor;
			let.Opacity = opacity;
			let.BorderColor = Color.Black;

			ShapeList.Add(let);
		}

		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
						
					return ShapeList[i];
				}	
			}
			return null;
		}
		
		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{

			foreach (var shape in selection)
			{
				shape.Move(p,lastLocation);
			}
			lastLocation = p;
		}

		internal void SetLineWidth(int size)
		{
			foreach (var shape  in Selection)
			{
				if (shape.GetType().Equals(typeof(GroupShape)))
				{
					((GroupShape)shape).SetLineWidth(size);
					continue;
				}
				shape.LineWidth = size;
			}

		}

		internal void SetColor(Color shapeColor)
		{
			foreach (var shape in Selection)
			{
				if (shape.GetType().Equals(typeof(GroupShape)))
				{
					((GroupShape)shape).SetFillColor(shapeColor);
					continue;
				}
				shape.FillColor = shapeColor;
			}

		}

		internal void SetOpacity(string opct, Shape copyShape)
		{
			int op = GetOpacityValue(opct);
			if (copyShape!=null)
			{
				copyShape.Opacity = op;
			}
			else
			{
				foreach (var shape in Selection)
				{
					if (shape.GetType().Equals(typeof(GroupShape)))
					{
						((GroupShape)shape).SetGroupOpacity(op);
						continue;
					}
					shape.Opacity = op;
				}
			}

		}

		private int GetOpacityValue(string opct)
		{
			int opcty = 55;
			switch (opct)
			{
				case "100%":
					opcty = 255;
					break;
				case "75%":
					opcty = 191;
					break;
				case "50%":
					opcty = 127;
					break;
				case "25%":
					opcty = 63;
					break;
				case "0":
					opcty = 0;
					break;
				default:
					opcty = 255;
					break;
			}
			return opcty;
		}

        internal void RotateShape(int value, Shape copyShape)
        {
			if (copyShape!=null)
			{
				copyShape.ShapeAngle = value;
			}
			else
			{

				foreach (var shape in Selection)
				{
					if (shape.GetType().Equals(typeof(GroupShape)))
					{
						((GroupShape)shape).RotateGroup(value);
						continue;
					}
					shape.ShapeAngle = (float)value;
				}
			}
		}

		internal void ChangeWidth(int value)
		{

			foreach (var shape in Selection)
			{
				if (shape.GetType().Equals(typeof(GroupShape)))
				{
					((GroupShape)shape).ChangeWidthGroup(value);
					continue;
				}
				shape.Width =value;
			}
		}

		internal void ChangeHeigth(int value)
		{

			foreach (var shape in Selection)
			{
				if (shape.GetType().Equals(typeof(GroupShape)))
				{
					((GroupShape)shape).ChangeHeigthGroup(value);
					continue;
				}
				shape.Height = value;
			}
		}

		internal void GroupSelectedShapes()
		{
			if (Selection.Count > 1)
			{

				if (Selection.OfType<GroupShape>().Any())
				{
					UnGroupSelectedShapes();
				}

				float Xmax = float.MinValue;
				float Xmin = float.MaxValue;
				float Ymax = float.MinValue;
				float Ymin = float.MaxValue;

				foreach (var s in Selection)
				{
					if (Xmin>s.Location.X)
					{
						Xmin = s.Location.X;
					}

					if (Xmax<s.Location.X+s.Width)
					{
						Xmax = s.Location.X + s.Width;
					}

					if (Ymin>s.Location.Y)
					{
						Ymin = s.Location.Y;
					}

					if (Ymax<s.Location.Y+s.Height)
					{
						Ymax = s.Location.Y + s.Height;
					}
				}
				GroupShape gs = new GroupShape(new RectangleF(Xmin,Ymin,Xmax - Xmin,Ymax-Ymin));
				gs.Shapes = Selection;

				foreach (var item in Selection)
				{
					ShapeList.Remove(item);
				}
					Selection = new List<Shape>();
				Selection.Add(gs);
				ShapeList.Add(gs);
			}
			else{
				return;
			}
		}

		internal void UnGroupSelectedShapes()
		{
			for (int i = 0; i < Selection.Count; i++)
			{
				if (Selection[i].GetType().Equals(typeof(GroupShape)))
				{
					GroupShape gs = (GroupShape)Selection[i];

					ShapeList.AddRange(gs.Shapes);
					Selection.AddRange(gs.Shapes);
					gs.Shapes.Clear();
					gs.Rectangle = new RectangleF();
					Selection[i] = null;
					gs = null;
					ShapeList.Remove(Selection[i]);
					Selection.Remove(Selection[i]);
				}
			}
		}

		internal void SearchByName(string name)
		{
			RemoveSelectedItems();
			foreach (var s in ShapeList)
			{
				if (s.ShapeName!=null)
				{
					if (s.ShapeName.Equals(name))
					{
						Selection.Add(s);
						s.BorderColor = Color.Red;
					}

				}

			}
		}

		private void RemoveSelectedItems()
		{
			for (int i = 0; i < Selection.Count; i++)
			{
				Selection[i].BorderColor = Color.Black;
			}
			Selection.Clear();
		}

		internal void SearchByColor(string color)
		{
			RemoveSelectedItems();
			if (color.Contains(","))
			{
				string[] colors = color.Split(',');
				if (colors.Length>=3)
				{
					int r = Int32.Parse(colors[0]);
					int g = Int32.Parse(colors[1]);
					int b = Int32.Parse(colors[2]);
					Color sColor = Color.FromArgb(r, g, b);
					foreach (var s in ShapeList)
					{
						if (s.FillColor == sColor)
						{
							Selection.Add(s);
							s.BorderColor = Color.Red;
						}
					}
				}
			}
			else
			{
				Color sColor = Color.FromName(color);
				foreach (var s in ShapeList)
				{
					if (s.FillColor==sColor)
					{
						Selection.Add(s);
						s.BorderColor = Color.Red;
					}
				}

			}
		}


		internal void SetColor(string color, Shape copyShape)
		{
			if (color.Contains(","))
			{
				string[] colors = color.Split(',');
				if (colors.Length >= 3)
				{
					int r = Int32.Parse(colors[0]);
					int g = Int32.Parse(colors[1]);
					int b = Int32.Parse(colors[2]);
					Color sColor = Color.FromArgb(r, g, b);
					if (copyShape!=null)
					{
						if (copyShape.GetType().Equals(typeof(GroupShape)))
						{
							((GroupShape)copyShape).SetFillColor(sColor);

						}
						else
						{
							copyShape.FillColor = sColor;
						}
						
					}
					else
					{
						foreach (var s in Selection)
						{
							if (s.GetType().Equals(typeof(GroupShape)))
							{
								((GroupShape)s).SetFillColor(sColor);
								continue;
							}
							s.FillColor = sColor;
						}
					}
				}
			}
			else
			{
				Color sColor = Color.FromName(color);
				if (copyShape != null)
				{
					if (copyShape.GetType().Equals(typeof(GroupShape)))
					{
						((GroupShape)copyShape).SetFillColor(sColor);

					}
					else
					{
						copyShape.FillColor = sColor;
					}
				}
				else
				{
					foreach (var s in Selection)
					{
						if (s.GetType().Equals(typeof(GroupShape)))
						{
							((GroupShape)s).SetFillColor(sColor);
							continue;
						}
						s.FillColor = sColor;
					}
				}

			}
		}

		internal void PasteItems(PointF mouseLocation)
		{
			foreach (var s in shapeCopies)
			{
				Shape copy = (Shape)s.Clone();
				copy.Location = mouseLocation;
				copy.BorderColor = Color.Black;
				ShapeList.Add(copy);
			}
		}

		internal void ClearAll()
		{
			ShapeList.Clear();
			Selection.Clear();
		}

		internal void CopyItems()
		{
			if (Selection.Count>0)
			{
				shapeCopies.Clear();
			}
			foreach (var s in Selection)
			{
				shapeCopies.Add(s);
			}
		}

		internal void CopyItems(Shape copyShape)
		{
			if (Selection.Count > 0)
			{
				shapeCopies.Clear();
			}
			shapeCopies.Add(copyShape);
		}

		internal void DeleteSelected(Shape copyShape)
		{
			if (copyShape!=null)
			{
				Selection.Remove(copyShape);
				ShapeList.Remove(copyShape);
			}
			{
				foreach (var shape in Selection)
				{
					ShapeList.Remove(shape);
				}
				Selection.Clear();
			}
		}

		internal void SearchByType(string type)
		{
			Selection.Clear();
			foreach (var s in ShapeList)
			{
				if (s.GetType().Name.Equals(type))
				{
					Selection.Add(s);
					s.BorderColor = Color.Red;
				}
				else
				{
					s.BorderColor = Color.Black;
				}
			}
		}

		internal void SetName(string name, Shape copyShape)
		{
			if (copyShape!=null)
			{
				copyShape.ShapeName = name;
			}
			else
			{
				foreach (var s in selection)
				{
					s.ShapeName = name;
				}
			}
		}
	}
}
