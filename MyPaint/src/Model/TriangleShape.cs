using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class TriangleShape : Shape
    {
        #region Constructors

        public TriangleShape()
        {
        }

        public TriangleShape(RectangleF rect) : base(rect)
        {
        }

        public TriangleShape(Shape shape) : base(shape)
        {
        }

        #endregion

        private Point[] points;
        public Point[] Points 
        {
            get { return points; }
            set { points = value; } 
        }
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                int max_point = Points.Length - 1;
                float total_angle = Angle(
                    Points[max_point].X, Points[max_point].Y,
                    point.X, point.Y,
                    Points[0].X, Points[0].Y);

                for (int i = 0; i < max_point; i++)
                {
                    total_angle += Angle(
                        Points[i].X, Points[i].Y,
                        point.X, point.Y,
                        Points[i + 1].X, Points[i + 1].Y);
                }

                return (Math.Abs(total_angle) > 1);

            }
            else
            {
                return false;
            }
        }
        public static float Angle(float Ax, float Ay,
    float Bx, float By, float Cx, float Cy)
        {
            float dot_product = Product(Ax, Ay, Bx, By, Cx, Cy);

            float cross_product = Length(Ax, Ay, Bx, By, Cx, Cy);

            return (float)Math.Atan2(cross_product, dot_product);
        }
        private static float Product(float Ax, float Ay,
    float Bx, float By, float Cx, float Cy)
        {
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            return (BAx * BCx + BAy * BCy);
        }
        public static float Length(float Ax, float Ay,
    float Bx, float By, float Cx, float Cy)
        {
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            return (BAx * BCy - BAy * BCx);
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            base.Rotate(grfx);
            Point[] p = { new Point((int)Rectangle.X + ((int)Rectangle.Width / 2), (int)Rectangle.Y), new Point((int)Rectangle.X, (int)(Rectangle.Y + Rectangle.Height)), new Point((int)(Rectangle.X + Rectangle.Width), (int)(Rectangle.Y + Rectangle.Height)) };
            points = p;
            SolidBrush brush = new SolidBrush(Color.FromArgb(Opacity,FillColor));
            grfx.FillPolygon(brush, points);
            grfx.DrawPolygon(new Pen(BorderColor,LineWidth), points);


        }


    }

}
