using RealTimeGraphX.DataPoints;
using RealTimeGraphX.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RealTimeGraphX.WPF.Demo
{
    public class MainWindowVM
    {
        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> MultiController { get; set; }

        public MainWindowVM()
        {
            Controller = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Range.MinimumY = 0;
            Controller.Range.MaximumY = 1080;
            Controller.Range.MaximumX = TimeSpan.FromSeconds(10);
            Controller.Range.AutoY = true;

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series",
                Stroke = Colors.DodgerBlue,
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

            Stopwatch watch = new Stopwatch();
            watch.Start();

            Task.Factory.StartNew(() =>
            {
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

                    Thread.Sleep(30);
                }
            });
        }
    }
}
