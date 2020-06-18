using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class EighthShape:Shape
    {

        #region Constructors
        public EighthShape()
        {
        }

        public EighthShape(RectangleF rect) : base(rect)
        {
        }

        public EighthShape(Shape shape) : base(shape)
        {
        }
        #endregion
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                double a1 = Rectangle.Width / 4;
                double b1 = Rectangle.Height / 4;

                double RectX = Rectangle.Location.X + a1;
                double RectY = Rectangle.Location.Y + b1;

                double resultX = Math.Pow(point.X - RectX, 2) / Math.Pow(a1, 2);
                double resultY = Math.Pow(point.Y - RectY, 2) / Math.Pow(b1, 2);
                double result = resultX + resultY;

                double a2 = Rectangle.Width / 4;
                double b2 = Rectangle.Height / 4;

                double RectX2 = Rectangle.Location.X + a2;
                double RectY2 = Rectangle.Location.Y + (b2*3);

                double resultX2 = Math.Pow(point.X - RectX2, 2) / Math.Pow(a2, 2);
                double resultY2 = Math.Pow(point.Y - RectY2, 2) / Math.Pow(b2, 2);
                double result2 = resultX2 + resultY2;

                if (result<=1||result2 <= 1)
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
            grfx.FillEllipse(brush, Rectangle.X, Rectangle.Y, Rectangle.Width / 2, Rectangle.Height / 2);
            grfx.DrawEllipse(p, Rectangle.X,Rectangle.Y,Rectangle.Width/2,Rectangle.Height/2);
            grfx.FillEllipse(brush, Rectangle.X, Rectangle.Y + 100, Rectangle.Width / 2, Rectangle.Height / 2);
            grfx.DrawEllipse(p, Rectangle.X, Rectangle.Y+100, Rectangle.Width / 2, Rectangle.Height / 2);
        }
    }
}
