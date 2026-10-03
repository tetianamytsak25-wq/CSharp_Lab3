using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using InterpolationApp.Models;

namespace InterpolationApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public void Build(double newH1, double newH2)
        {
            H1 = newH1;
            H2 = newH2;
            Build();
        }
        private const double MinX = 0;
        private const double MaxX = 2 * Math.PI;

        private double h1 = 0.5;
        private double h2 = 0.1;

        private bool showReference = true;
        private bool showLinear = true;
        private bool showHermite = true;
        private bool showPoints = true;
        private bool showError = true;

        private string message = "";

        public double H1
        {
            get { return h1; }
            set
            {
                h1 = value;
                OnPropertyChanged();
            }
        }

        public double H2
        {
            get { return h2; }
            set
            {
                h2 = value;
                OnPropertyChanged();
            }
        }

        public bool ShowReference
        {
            get { return showReference; }
            set
            {
                showReference = value;
                OnPropertyChanged();
            }
        }

        public bool ShowLinear
        {
            get { return showLinear; }
            set
            {
                showLinear = value;
                OnPropertyChanged();
            }
        }

        public bool ShowHermite
        {
            get { return showHermite; }
            set
            {
                showHermite = value;
                OnPropertyChanged();
            }
        }

        public bool ShowPoints
        {
            get { return showPoints; }
            set
            {
                showPoints = value;
                OnPropertyChanged();
            }
        }

        public bool ShowError
        {
            get { return showError; }
            set
            {
                showError = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            private set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        public PointCollection ReferencePoints { get; private set; }
        public PointCollection LinearPoints { get; private set; }
        public PointCollection HermitePoints { get; private set; }
        public PointCollection SupportPoints { get; private set; }
        public PointCollection ErrorPoints { get; private set; }

        public ICommand BuildCommand { get; }

        public MainViewModel()
        {
            ReferencePoints = new PointCollection();
            LinearPoints = new PointCollection();
            HermitePoints = new PointCollection();
            SupportPoints = new PointCollection();
            ErrorPoints = new PointCollection();

            BuildCommand = new RelayCommand(
                parameter => Build());

            Build();
        }

        private void Build()
        {
            Message = $"Отримано h1 = {H1}, h2 = {H2}";
            if (H1 <= 0 || H2 <= 0)
            {
                Message = "Кроки повинні бути більшими за 0.";
                return;
            }

            if (H2 >= H1)
            {
                Message = "Помилка: потрібно, щоб h₂ < h₁.";
                return;
            }

            List<Point> a1 = CreatePoints(H1);
            List<Point> a2 = CreatePoints(H2);

            Interpolation linear = new LinearInterpolation();

            Interpolation hermite = new HermiteInterpolation();

            ReferencePoints = new PointCollection();
            LinearPoints = new PointCollection();
            HermitePoints = new PointCollection();
            SupportPoints = new PointCollection();
            ErrorPoints = new PointCollection();

            foreach (Point point in a2)
            {
                ReferencePoints.Add(ToScreenPoint(point.X, Math.Sin(point.X)));
            }
            foreach (Point point in a1)
            {
                SupportPoints.Add(ToScreenPoint(point.X, point.Y));
            }

            foreach (Point point in a2)
            {
                double linearY = linear.Calculate(a1, point.X);

                double hermiteY = hermite.Calculate(a1, point.X);

                LinearPoints.Add(ToScreenPoint(point.X, linearY));

                HermitePoints.Add(ToScreenPoint(point.X, hermiteY));

                double error =Math.Abs(Math.Sin(point.X) - hermiteY);

                ErrorPoints.Add(ToErrorScreenPoint(point.X, error));
            }
            Message = "Побудову виконано. Точок: " + SupportPoints.Count;
            OnPropertyChanged(nameof(ReferencePoints));
            OnPropertyChanged(nameof(LinearPoints));
            OnPropertyChanged(nameof(HermitePoints));
            OnPropertyChanged(nameof(SupportPoints));
            OnPropertyChanged(nameof(ErrorPoints));

            Message ="Побудову виконано. Умова h₂ < h₁ виконується.";
        }

        private List<Point> CreatePoints(double step)
        {
            List<Point> points = new List<Point>();

            double x = MinX;

            while (x < MaxX)
            {
                points.Add(new Point(x, Math.Sin(x)));

                x += step;
            }

            points.Add(new Point(MaxX, Math.Sin(MaxX)));

            return points;
        }

        private Point ToScreenPoint(double x, double y)
        {
            double screenX = 40 + x / MaxX * 720;

            double screenY = 160 - y * 130;

            return new Point(screenX, screenY);
        }

        private Point ToErrorScreenPoint(double x, double error)
        {
            double screenX = 40 + x / MaxX * 720;

            double screenY = 150 - error * 5000;

            if (screenY < 20)
            {
                screenY = 20;
            }

            return new Point(screenX, screenY);
        }

        public event PropertyChangedEventHandler?
            PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}