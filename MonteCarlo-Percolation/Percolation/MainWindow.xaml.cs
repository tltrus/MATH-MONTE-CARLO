using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Percolation
{
    
    public partial class MainWindow : Window
    {
        class Cell
        {
            public bool opened;
            public bool percolated;
        }
        Random rand = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        int width, height;
        DrawingVisual visual;
        DrawingContext dc;

        int openSites, N, cellSize;
        Cell[,] cells;
        WQUPC WQUPC; // Quick union with path compression.

        int topvirtual;
        int bottomvirtual;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            Init();
            timer.Tick += Tick;
            timer.Interval = new TimeSpan(0,0,0,0,10);
        }

        private void Init()
        {
            N = 50;
            cellSize = 11;

            openSites = 0;
            topvirtual = 0;
            bottomvirtual = 0;

            // cells setup
            cells = new Cell[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    cells[i, j] = new Cell();
                }
            }

            WQUPC = new WQUPC(N *  N + 2);
        }


        private bool isPercolates() => WQUPC.isConnected(N * N, N * N + 1);

        private int Index(int row, int column) => column + row * N;

        private bool isOpen(int row, int column) => cells[row, column].opened;

        private void Open(int row, int column)
        {
            if (cells[row, column].opened) return;
            else
            {
                cells[row, column].opened = true;
                openSites++;

                // Соединение со всеми открытыми соседями
                //Connected to Virtual Top
                if (row - 1 == -1)
                {
                    WQUPC.Union(Index(row, column), N * N);
                }
                //Connected to Virtual Bottom
                if (row + 1 == N)
                {
                    WQUPC.Union(Index(row, column), N * N + 1);
                }

                //Connected to Left 
                if (column >= 1 && cells[row, column - 1].opened)
                {
                    WQUPC.Union(Index(row, column), Index(row, column - 1));
                }
                //Connected to Right
                if (column <= N - 2 && cells[row, column + 1].opened)
                {
                    WQUPC.Union(Index(row, column), Index(row, column + 1))
;
                }
                //Connected to top
                if (row >= 1 && cells[row - 1, column].opened)
                {
                    WQUPC.Union(Index(row, column), Index(row - 1, column));
                }
                //Connected to Bottom
                if (row <= N - 2 && cells[row + 1, column].opened)
                {
                    WQUPC.Union(Index(row, column), Index(row + 1, column));
                }
            }
        }
        
        private void Tick(object sender , EventArgs e)
        {
            if (!isPercolates())
            {
                int col = rand.Next(N);
                int row = rand.Next(N);
                if (!isOpen(row, col))
                {
                    Open(row, col);
                }
            }
            else
            {
                timer.Stop();
                double p = openSites / (double)(N * N);
                rtbP.AppendText("Probability is " + p.ToString() + "\n");
            }

            topvirtual = WQUPC.id[N * N];
            bottomvirtual = WQUPC.id[N * N + 1];

            // Установка признака просочившейся клетки
            for (int a = 0; a < N * N; ++a)
            {
                int r = a / N;
                int c = a % N;

                // поиск среди открытых клеток
                if (cells[r, c].opened)
                {
                    // поиск клетки, у которой значение совпадает с виртуальной top.
                    // или же поиск общего родителя у клетки и у виртуальной топовой, чтобы доказать их связь и залить цветом
                    if (WQUPC.id[a] == topvirtual || WQUPC.isConnected(WQUPC.id[a], topvirtual))
                    {
                        cells[r, c].percolated = true;
                    }
                }
            }
            Drawing();
        }

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                Brush brush = Brushes.Black;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (cells[i, j].percolated)
                        {
                            brush = new SolidColorBrush(Color.FromRgb(129,165,213));
                        }
                        else 
                        if (cells[i, j].opened)
                        {
                            brush = Brushes.White;
                        }
                        else
                        {
                            brush = Brushes.Black;
                        }

                        Rect rect = new Rect()
                        {
                            X = j * cellSize,
                            Y = i * cellSize,
                            Width = cellSize,
                            Height = cellSize,
                        };
                        
                        dc.DrawRectangle(brush, new Pen(brush, 1), rect);
                    }
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Init();
            timer.Start();
        }
    }
}
