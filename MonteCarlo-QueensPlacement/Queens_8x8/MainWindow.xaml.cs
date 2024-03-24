using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp.Classes;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();
        double width, height;
        DrawingVisual visual;
        DrawingContext dc;

        int N = 8; // map's rows\cols numbers
        int[] Pos; // Queens position
        int[] s;

        double T = 100; // temperature
        double alpha = 0.95;
        int L = 1000;

        Map Map;
        


        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = g.Width;
            height = g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

           
            Init();
        }

        private void Init()
        {
            Map = new Map(8, width, height);

            T = 100;

            Pos = new int[N];
            s = new int[N];

            // Заполнение массив положений ферзей
            for (int i = 0; i < N; i++)
            {
                Pos[i] = i;
                s[i] = i;
            }
        }


        private void timerTick(object sender, EventArgs e)
        {
            Update();
            Draw();
        }

        private void Draw()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                // Draw map
                Map.Draw(dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void Update()
        {
            Dynamic_Annealing();
            Map.SetQueensPosition(s);

            lbH.Content = H(s).ToString();
            lbT.Content = Math.Round(T, 3).ToString();
        }

        // Annealing
        private void Dynamic_Annealing()
        {
            if (H(s) == 0) timer.Stop();

            int[] s_ = G(s);

            double delta = H(s_) - H(s);

            if (delta < 0)
                Array.Copy(s_, s, s_.Length);   // s = s_;
            else
            {
                // вычисляем вероятность перехода
                double p = Math.Exp(-delta / T);

                if (rnd.NextDouble() <= p)
                    Array.Copy(s, s_, s.Length);   // s_ = s;
            }

            T = alpha * T;
        }
        private int H(int[] pos)
        {
            int res = 0, k = 0;

            for (int n = 0; n < N; n++)
            {
                k = n - 1;
                while (k >= 0)
                {
                    if (pos[k] == (pos[n] + (n - k)))
                        res = res + 1;

                    if (pos[k] == (pos[n] - (n - k)))
                        res = res + 1;
                    k = k - 1;
                }
                k = n + 1;

                while (k < N)
                {
                    if (pos[k] == (pos[n] + (k - n)))
                        res = res + 1;

                    if (pos[k] == (pos[n] - (k - n)))
                        res = res + 1;
                    k = k + 1;
                }
            }
            return res;
        }
        private int[] G(int[] Pos)
        {
            int[] pos = new int[Pos.Length];
            Array.Copy(Pos, pos, Pos.Length);   // pos = Pos;
            int i = 0;
            int j = 0;
            while (i == j)
            {
                i = rnd.Next(0, N);
                j = rnd.Next(0, N);
            }

            int a = pos[i];
            pos[i] = pos[j];
            pos[j] = a;

            return pos;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Init();
            Draw();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
