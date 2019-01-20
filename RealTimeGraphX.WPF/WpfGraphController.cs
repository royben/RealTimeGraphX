using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a WPF <see cref="GraphController{TDataSeries, TXDataPoint, TYDataPoint}">graph controller</see>.
    /// </summary>
    /// <typeparam name="TXDataPoint">The type of the x data point.</typeparam>
    /// <typeparam name="TYDataPoint">The type of the y data point.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphController{RealTimeGraphX.WPF.WpfGraphDataSeries, TXDataPoint, TYDataPoint}" />
    public class WpfGraphController<TXDataPoint, TYDataPoint> : GraphController<WpfGraphDataSeries, TXDataPoint, TYDataPoint>
        where TXDataPoint : GraphDataPoint
        where TYDataPoint : GraphDataPoint
    {

    }
}
