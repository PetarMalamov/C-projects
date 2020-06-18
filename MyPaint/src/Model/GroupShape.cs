using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class GroupShape : Shape
    {
        #region Constructors
        public GroupShape()
        {
        }

        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(Shape shape) : base(shape)
        {
        }
        #endregion
        private List<Shape> shapes = new List<Shape>();

        #region Props
        public List<Shape> Shapes
        {
            get { return shapes; }
            set { shapes = value; }
        }

        public override RectangleF Rectangle {
            get => base.Rectangle; set => base.Rectangle = value; 
        }
        public override float Width { 
            get => base.Width; set => base.Width = value; 
        }
        public override float Height { 
            get => base.Height; set => base.Height = value; 
        }
        public override PointF Location {
            get => base.Location; set => base.Location = value; 
        }
        public override Color FillColor {
            get => base.FillColor; set => base.FillColor = value; 
        }
        public override int LineWidth {
            get => base.LineWidth; set => base.LineWidth = value; 
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                foreach (var shape in shapes)
                {
                    if (shape.Contains(point))
                    {
                        return true;
                    }
                //return false;
                }
            }
            return false;
        }

        public override void DrawSelf(Graphics grfx)
        {

            base.DrawSelf(grfx);
            grfx.DrawRectangle(new Pen(BorderColor, LineWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            foreach (var shape in shapes)
            {
                shape.DrawSelf(grfx);
            }
        }

        public void RotateGroup(float angle)
        {
            foreach (var shape in shapes)
            {
                shape.ShapeAngle = angle;
            }

        }

        public virtual void SetFillColor(Color color)
        {
            FillColor = color;
            foreach (var shape in shapes)
            {
                shape.FillColor = color;
            }
        }

        public virtual void GruopMove(PointF point)
        {
            foreach (var shape in shapes)
            {
                shape.Location = new PointF(this.Location.X + (shape.Location.X - point.X), this.Location.Y - (point.Y - shape.Location.Y));
            }
        }

        public override void Move(PointF p, PointF lastLocation)
        {
            base.Move(p, lastLocation);
            foreach (var s in shapes)
            {
                s.Move(p, lastLocation);
            }
        }

        public override void Rotate(Graphics g)
        {
            base.Rotate(g);
        }

        internal void ChangeWidthGroup(int value)
        {
            float Xmax = float.MinValue;
            float Xmin = float.MaxValue;
            float Ymax = float.MinValue;
            float Ymin = float.MaxValue;

            foreach (var s in shapes)
            {
                s.Width = value;
                if (Xmin > s.Location.X)
                {
                    Xmin = s.Location.X;
                }

                if (Xmax < s.Location.X + s.Width)
                {
                    Xmax = s.Location.X + s.Width;
                }

                if (Ymin > s.Location.Y)
                {
                    Ymin = s.Location.Y;
                }

                if (Ymax < s.Location.Y + s.Height)
                {
                    Ymax = s.Location.Y + s.Height;
                }
            }

            Rectangle = new RectangleF(Xmin, Ymin, Xmax - Xmin, Ymax - Ymin);
        }

        internal void SetLineWidth(int size)
        {
            foreach (var s in shapes)
            {
                s.LineWidth = size;
            }
        }

        internal void ChangeHeigthGroup(int value)
        {
            float Xmax = float.MinValue;
            float Xmin = float.MaxValue;
            float Ymax = float.MinValue;
            float Ymin = float.MaxValue;

            foreach (var s in shapes)
            {
                s.Height = value;
                if (Xmin > s.Location.X)
                {
                    Xmin = s.Location.X;
                }

                if (Xmax < s.Location.X + s.Width)
                {
                    Xmax = s.Location.X + s.Width;
                }

                if (Ymin > s.Location.Y)
                {
                    Ymin = s.Location.Y;
                }

                if (Ymax < s.Location.Y + s.Height)
                {
                    Ymax = s.Location.Y + s.Height;
                }
            }

            Rectangle = new RectangleF(Xmin, Ymin, Xmax - Xmin, Ymax - Ymin);
        }

        internal void ResizeGroup()
        {
            float Xmax = float.MinValue;
            float Xmin = float.MaxValue;
            float Ymax = float.MinValue;
            float Ymin = float.MaxValue;

            foreach (var s in shapes)
            {
                if (Xmin > s.Location.X)
                {
                    Xmin = s.Location.X;
                }

                if (Xmax < s.Location.X + s.Width)
                {
                    Xmax = s.Location.X + s.Width;
                }

                if (Ymin > s.Location.Y)
                {
                    Ymin = s.Location.Y;
                }

                if (Ymax < s.Location.Y + s.Height)
                {
                    Ymax = s.Location.Y + s.Height;
                }
            }

            Rectangle = new RectangleF(Xmin, Ymin, Xmax - Xmin, Ymax - Ymin);
        }

        internal void SetGroupOpacity(int opct)
        {
            foreach (var s in shapes)
            {
                s.Opacity = opct;
            }
        }
    }
}
