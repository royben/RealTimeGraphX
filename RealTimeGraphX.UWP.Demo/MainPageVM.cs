using RealTimeGraphX.DataPoints;
using RealTimeGraphX.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP.Demo
{
    public class MainPageVM : GraphObject
    {
        public UwpGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public UwpGraphController<TimeSpanDataPoint, DoubleDataPoint> MultiController { get; set; }

        public MainPageVM()
        {
            Controller = new UwpGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Range.MinimumY = 0;
            Controller.Range.MaximumY = 1080;
            Controller.Range.MaximumX = TimeSpan.FromSeconds(10);
            Controller.Range.AutoY = true;

            Controller.DataSeriesCollection.Add(new UwpGraphDataSeries()
            {
                Name = "Series",
                Stroke = Colors.DodgerBlue,
            });

            MultiController = new UwpGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            MultiController.Range.MinimumY = 0;
            MultiController.Range.MaximumY = 1080;
            MultiController.Range.MaximumX = TimeSpan.FromSeconds(10);
            MultiController.Range.AutoY = true;

            MultiController.DataSeriesCollection.Add(new UwpGraphDataSeries()
            {
                Name = "Series 1",
                Stroke = Colors.Red,
            });

            MultiController.DataSeriesCollection.Add(new UwpGraphDataSeries()
            {
                Name = "Series 2",
                Stroke = Colors.Green,
            });

            MultiController.DataSeriesCollection.Add(new UwpGraphDataSeries()
            {
                Name = "Series 3",
                Stroke = Colors.Blue,
            });

            MultiController.DataSeriesCollection.Add(new UwpGraphDataSeries()
            {
                Name = "Series 4",
                Stroke = Colors.Yellow,
            });

            MultiController.DataSeriesCollection.Add(new UwpGraphDataSeries()
            {
                Name = "Series 5",
                Stroke = Colors.Gray,
            });

            Stopwatch watch = new Stopwatch();
            watch.Start();

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    double y = 0;

                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        y = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition.Y;
                    });

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
