using System.Collections.Generic;
using System.Windows;

namespace InterpolationApp.Models
{
    public abstract class Interpolation
    {
        public abstract double Calculate(IReadOnlyList<Point> points, double x);
    }
}