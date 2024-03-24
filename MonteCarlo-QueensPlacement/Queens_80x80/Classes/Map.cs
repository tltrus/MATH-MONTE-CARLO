using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfApp.Classes
{
    internal class Map
    {
        int N;
        double width, height;
        int cellSize = 10;
        Brush brush;
        List<Queen> Queens;
        Pen pen;

        public Map(int n, double width, double height)
        {
            N = n;
            brush = Brushes.White;
            this.width = width;
            this.height = height;
            Queens = new List<Queen>();

            for (int i = 0; i < n; i++)
            {
                Queens.Add(new Queen());
            }

            pen = new Pen(brush, 0.2);
        }

        public void SetQueensPosition(int[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                double x = (s[i] * cellSize) + (cellSize * 0.5);
                double y = (i * cellSize) + (cellSize * 0.5);

                Queens[i].SetPosition(x, y);
            }
        }

        public void Draw(DrawingContext dc)
        {
            // Draw rows and cols
            for (int i = 0; i < N; i++)
            {
                Point p0 = new Point(0, i * cellSize);
                Point p1 = new Point(width, i * cellSize);

                dc.DrawLine(pen, p0, p1);

                p0 = new Point(i * cellSize, 0);
                p1 = new Point(i * cellSize, height);

                dc.DrawLine(pen, p0, p1);
            }

            // Draw queens
            foreach (var q in Queens)
            {
                q.Draw(dc);
            }
        }
    }
}
