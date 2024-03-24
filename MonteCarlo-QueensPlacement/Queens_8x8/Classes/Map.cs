using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp.Classes
{
    internal class Map
    {
        int rows, cols;
        double width, height;
        int cellSize = 50;
        Brush brush;
        List<Queen> Queens;

        public Map(int n, double width, double height)
        {
            rows = cols = n;
            brush = Brushes.White;
            this.width = width;
            this.height = height;
            Queens = new List<Queen>();

            for (int i = 0; i < n; i++)
            {
                Queens.Add(new Queen());
            }
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
            // Draw rows
            for (int i = 0; i < rows; i++)
            {
                Pen pen = new Pen(brush, 0.7);
                Point p0 = new Point(0, i * cellSize);
                Point p1 = new Point(width, i * cellSize);

                dc.DrawLine(pen, p0, p1);
            }

            // Draw cols
            for (int i = 0; i < cols; i++)
            {
                Pen pen = new Pen(brush, 0.7);
                Point p0 = new Point(i * cellSize, 0);
                Point p1 = new Point(i * cellSize, height);

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
