using System.Collections.Generic;
using System.Windows;

namespace InterpolationApp.Models
{
    public sealed class HermiteInterpolation : Interpolation
    {
        public override double Calculate(IReadOnlyList<Point> points, double x)
        {
            if (points.Count < 2)
            {
                return 0;
            }

            if (x <= points[0].X)
            {
                return points[0].Y;
            }

            if (x >= points[points.Count - 1].X)
            {
                return points[points.Count - 1].Y;
            }

            int index = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (x >= points[i].X && x <= points[i + 1].X)
                {
                    index = i;
                    break;
                }
            }

            Point p0 = points[index];
            Point p1 = points[index + 1];

            double dx = p1.X - p0.X;

            double t = (x - p0.X) / dx;

            double m0 = GetDerivative(points, index);
            double m1 = GetDerivative(points, index + 1);

            double h00 = 2 * t * t * t - 3 * t * t + 1;

            double h10 = t * t * t - 2 * t * t + t;

            double h01 = -2 * t * t * t + 3 * t * t;

            double h11 = t * t * t - t * t;

            return h00 * p0.Y + h10 * dx * m0 + h01 * p1.Y + h11 * dx * m1;
        }

        private double GetDerivative(IReadOnlyList<Point> points, int index)
        {
            if (index == 0)
            {
                return (points[1].Y - points[0].Y) / (points[1].X - points[0].X);
            }

            if (index == points.Count - 1)
            {
                int last = points.Count - 1;

                return (points[last].Y - points[last - 1].Y) / (points[last].X - points[last - 1].X);
            }

            return (points[index + 1].Y - points[index - 1].Y) / (points[index + 1].X - points[index - 1].X);
        }
    }
}