using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    class LetterShape : Shape
    {
        public LetterShape()
        {
        }

        public LetterShape(RectangleF rect) : base(rect)
        {
        }

        public LetterShape(Shape shape) : base(shape)
        {
        }

        public override RectangleF Rectangle { get => base.Rectangle; set => base.Rectangle = value; }
        public override float Width { get => base.Width; set => base.Width = value; }
        public override float Height { get => base.Height; set => base.Height = value; }
        public override PointF Location { get => base.Location; set => base.Location = value; }
        public override Color FillColor { get => base.FillColor; set => base.FillColor = value; }
        public override int LineWidth { get => base.LineWidth; set => base.LineWidth = value; }

        public override object Clone()
        {
            return base.Clone();
        }

        public override bool Contains(PointF point)
        {
            return base.Contains(point);
        }

        public override void DrawSelf(Graphics g)
        {
            base.DrawSelf(g);
            base.Rotate(g);

            g.FillRectangle(new SolidBrush(Color.FromArgb(Opacity, FillColor)), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.DrawRectangle(new Pen(BorderColor, LineWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.DrawLine(new Pen(BorderColor, LineWidth), Rectangle.X, Rectangle.Y, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + (Rectangle.Height / 2));
            g.DrawLine(new Pen(BorderColor, LineWidth), Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + (Rectangle.Height / 2));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Move(PointF p, PointF lastLocation)
        {
            base.Move(p, lastLocation);
        }

        public override void Rotate(Graphics g)
        {
            base.Rotate(g);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
