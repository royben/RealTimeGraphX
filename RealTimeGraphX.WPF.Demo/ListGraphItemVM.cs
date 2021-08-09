using RealTimeGraphX.DataPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RealTimeGraphX.WPF.Demo
{
    public class ListGraphItemVM
    {
        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public String Name { get; set; }

        public String StringFormat { get; set; }

        public Color Color
        {
            get { return Controller.DataSeriesCollection[0].Stroke; }
            set { Controller.DataSeriesCollection[0].Stroke = value; }
        }

        public ListGraphItemVM()
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
                Stroke = Colors.DodgerBlue,
            });

            StringFormat = "0.0";
        }
    }
}
