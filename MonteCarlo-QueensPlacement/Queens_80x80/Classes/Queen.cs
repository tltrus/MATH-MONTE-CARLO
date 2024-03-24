using System.Windows;
using System.Windows.Media;

namespace WpfApp.Classes
{
    internal class Queen
    {
        Brush brush;
        Pen pen;
        double r;
        Point pos;

        public Queen() 
        {
            brush = Brushes.LightGreen;
            pen = new Pen(Brushes.White, 0.2);
            r = 3;
            pos = new Point();
        }

        public void SetPosition(double x, double y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Draw(DrawingContext dc)
        {
            if (pos.X == 0 || pos.Y == 0) return;

            dc.DrawEllipse(brush, pen, pos, r, r);
        }
    }
}
