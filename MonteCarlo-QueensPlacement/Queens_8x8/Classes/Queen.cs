using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            pen = new Pen(Brushes.White, 1);
            r = 20;
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
