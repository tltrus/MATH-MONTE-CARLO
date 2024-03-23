using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MCarlo
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();
        DrawingVisual visual;
        DrawingContext dc;
        double width, height;
        Axis axis;
        Point mouse;
        int factor = 25;

        double a = 0.5;
        double b = 2.1;
        double h = 1.5;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            width = g.Width;
            height = g.Height;

            mouse = new Point();
            visual = new DrawingVisual();

            axis = new Axis(width, height);

            Drawing();
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = e.GetPosition(g);
            Drawing();
        }
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                factor -= 1;
            }
            else
            {
                // increasing
                factor += 1;
            }

            if (factor <= 0) factor = 1;
            if (factor >= 58) factor = 58;

            Drawing();
        }
        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                // axis drawing
                axis.Draw(dc, visual);

                axis.SetFactor(factor);

                // func
                double y = 0;
                PointCollection points = new PointCollection();

                for (int i = -360; i < 360; i += 5)
                {
                    double x = i * Math.PI / 180;
                    y = Math.Sin(x);
                    points.Add(new Point(axis.Xto(x), axis.Yto(y)));
                }

                StreamGeometry streamGeometry = new StreamGeometry();
                using (StreamGeometryContext geometryContext = streamGeometry.Open())
                {
                    Point startpoint = points[0];
                    points.RemoveAt(0);
                    geometryContext.BeginFigure(startpoint, false, false);
                    geometryContext.PolyLineTo(points, true, true);
                }
                dc.DrawGeometry(null, new Pen(Brushes.Red, 2), streamGeometry);

                DrawGreenArea(dc);
                DrawRedArea(dc);
                DropPoints(dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }
        private void DrawGreenArea(DrawingContext dc)
        {
            double y = 0;
            double i = 0;

            PointCollection points = new PointCollection();
            points.Add(new Point(axis.Xto(a), axis.Yto(0)));

            for (i = a; i <= b + 0.1; i += 0.1)
            {
                y = Math.Sin(i);
                points.Add(new Point(axis.Xto(i), axis.Yto(y)));
            }
            points.Add(new Point(axis.Xto(b), axis.Yto(0)));

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                Point startpoint = points[0];
                points.RemoveAt(0);
                geometryContext.BeginFigure(startpoint, true, true);
                geometryContext.PolyLineTo(points, true, true);
            }
            dc.DrawGeometry(Brushes.LightGreen, null, streamGeometry);
        }
        private void DrawRedArea(DrawingContext dc)
        {
            double y = 0;
            double i = 0;

            PointCollection points = new PointCollection();
            points.Add(new Point(axis.Xto(a), axis.Yto(h)));

            for (i = a; i <= b + 0.1; i += 0.1)
            {
                y = Math.Sin(i);
                points.Add(new Point(axis.Xto(i), axis.Yto(y)));
            }
            points.Add(new Point(axis.Xto(b), axis.Yto(h)));

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                Point startpoint = points[0];
                points.RemoveAt(0);
                geometryContext.BeginFigure(startpoint, true, true);
                geometryContext.PolyLineTo(points, true, true);
            }
            dc.DrawGeometry(Brushes.LightPink, null, streamGeometry);
        }
        private void DropPoints(DrawingContext dc)
        {
            // Filling the areas at the top and bottom of the graph with dots on the segment [a, b]
            double N = 200; // all points num
            double redNum = 0;
            double greenNum = 0;
            for (int i = 0; i < N; i++)
            {
                double pX_ = rnd.Next((int)(a * 10), (int)(b * 10)); // *10 is a trick to avoid double number in random
                double pX = pX_ / 10;
                double pY_ = rnd.Next(0, (int)(h * 10));
                double pY = pY_ / 10;

                Brush color;
                if (pY <= Math.Sin(pX))
                {
                    color = Brushes.Green;
                    greenNum++;
                }
                else
                {
                    color = Brushes.Red;
                    redNum++;
                }

                Point point = new Point(axis.Xto(pX), axis.Yto(pY));
                dc.DrawEllipse(color, null, point, 1, 1);
            }

            // Square (integration) is: S = n / N * (b - a) * h
            double S = (greenNum / N) * (b - a) * h;

            lbMCarlo.Content = S.ToString();
        }
    }
}