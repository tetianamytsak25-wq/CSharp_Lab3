using System.Collections.Generic;
using System.Windows;

namespace InterpolationApp.Models
{
    public sealed class LinearInterpolation : Interpolation
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

            for (int i = 0; i < points.Count - 1; i++)
            {
                double x1 = points[i].X;
                double y1 = points[i].Y;

                double x2 = points[i + 1].X;
                double y2 = points[i + 1].Y;

                if (x >= x1 && x <= x2)
                {
                    double t = (x - x1) / (x2 - x1);

                    return y1 + t * (y2 - y1);
                }
            }

            return 0;
        }
    }
}