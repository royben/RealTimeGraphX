using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents a UWP <see cref="IGraphController{TDataSeries, TXDataPoint, TYDataPoint}">graph controller</see>.
    /// </summary>
    /// <typeparam name="TXDataPoint">The type of the x data point.</typeparam>
    /// <typeparam name="TYDataPoint">The type of the y data point.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphController{RealTimeGraphX.UWP.UwpGraphDataSeries, TXDataPoint, TYDataPoint}" />
    public class UwpGraphController<TXDataPoint, TYDataPoint> : GraphController<UwpGraphDataSeries, TXDataPoint, TYDataPoint>
        where TXDataPoint : GraphDataPoint
        where TYDataPoint : GraphDataPoint
    {

    }
}
