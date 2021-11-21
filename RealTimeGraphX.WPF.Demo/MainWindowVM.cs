using RealTimeGraphX.DataPoints;
using RealTimeGraphX.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RealTimeGraphX.WPF.Demo
{
    public class MainWindowVM
    {
        private Random r = new Random();

        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> MultiController { get; set; }

        public List<ListGraphItemVM> ListItems { get; set; }

        public List<ListGraphItemVM> ListItems2 { get; set; }

        public MainWindowVM()
        {
            Controller = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Range.MinimumY = 0;
            Controller.Range.MaximumY = 1080;
            Controller.Range.MaximumX = TimeSpan.FromSeconds(10);
            Controller.Range.AutoY = true;
            Controller.Range.AutoYFallbackMode = GraphRangeAutoYFallBackMode.MinMax;

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series",
                Stroke = Colors.Red,
            });

            MultiController = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            MultiController.Range.MinimumY = 0;
            MultiController.Range.MaximumY = 1080;
            MultiController.Range.MaximumX = TimeSpan.FromSeconds(10);
            MultiController.Range.AutoY = true;

            MultiController.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series 1",
                Stroke = Colors.Red,
            });

            MultiController.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series 2",
                Stroke = Colors.Green,
            });

            MultiController.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series 3",
                Stroke = Colors.Blue,
            });

            MultiController.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series 4",
                Stroke = Colors.Yellow,
            });

            MultiController.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series 5",
                Stroke = Colors.Gray,
            });

            ListItems = new List<ListGraphItemVM>();

            for (int i = 1; i < 6; i++)
            {
                var item = new ListGraphItemVM();
                item.Name = $"Item {i}";
                item.StringFormat = $"F{i}";
                item.Color = GetRandomColor();
                ListItems.Add(item);
            }

            ListItems2 = new List<ListGraphItemVM>();

            for (int i = 1; i < 6; i++)
            {
                var item = new ListGraphItemVM();
                item.Name = $"Item {i}";
                item.StringFormat = $"F{i}";
                item.Color = GetRandomColor();
                ListItems2.Add(item);
            }

            Application.Current.MainWindow.ContentRendered += (_, __) => Start();
        }

        private void Start()
        {
            Task.Factory.StartNew(() =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                while (true)
                {
                    var y = System.Windows.Forms.Cursor.Position.Y;

                    List<DoubleDataPoint> yy = new List<DoubleDataPoint>()
                    {
                        y,
                        y + 20,
                        y + 40,
                        y + 60,
                        y + 80,
                    };

                    var x = watch.Elapsed;

                    List<TimeSpanDataPoint> xx = new List<TimeSpanDataPoint>()
                    {
                        x,
                        x,
                        x,
                        x,
                        x
                    };

                    Controller.PushData(x, y);
                    MultiController.PushData(xx, yy);

                    for (int i = 0; i < ListItems.Count; i++)
                    {
                        ListItems[i].Controller.PushData(x, y + r.Next(0, 50 * (i + 1)));
                        ListItems2[i].Controller.PushData(x, y + r.Next(0, 50 * (i + 1)));
                    }

                    Thread.Sleep(30);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte)r.Next(50, 255), (byte)r.Next(50, 255), (byte)r.Next(50, 255));
        }
    }
}
