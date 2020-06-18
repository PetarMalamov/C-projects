using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class ElipseShape : Shape
    {
        #region Constructors
        public ElipseShape()
        {
        }

        public ElipseShape(RectangleF rect) : base(rect)
        {
        }

        public ElipseShape(Shape shape) : base(shape)
        {
        }
        #endregion
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                double a = Rectangle.Width / 2;
                double b = Rectangle.Height / 2;

                double RectX = Rectangle.Location.X + a;
                double RectY = Rectangle.Location.Y + b;

                double resultX = Math.Pow(point.X - RectX, 2)/Math.Pow(a,2);
                double resultY = Math.Pow(point.Y - RectY, 2) / Math.Pow(b, 2);
                double result = resultX + resultY;

                if (result<=1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            base.Rotate(grfx);
            Pen p = new Pen(BorderColor, LineWidth);
            SolidBrush brush = new SolidBrush(Color.FromArgb(Opacity, FillColor));
            grfx.FillEllipse(brush, Rectangle);
            grfx.DrawEllipse(p, Rectangle);
        }
    }
}
