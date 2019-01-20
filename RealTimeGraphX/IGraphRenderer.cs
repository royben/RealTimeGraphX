using RealTimeGraphX.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents a graph renderer capable of receiving a series of data points from a controller and transforming them to drawing points.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <seealso cref="RealTimeGraphX.IGraphComponent" />
    public interface IGraphRenderer<TDataSeries> : IGraphComponent where TDataSeries : IGraphDataSeries
    {
        /// <summary>
        /// Arranges the series of data points and returns a series of drawing points.
        /// </summary>
        /// <param name="surface">The target graph surface.</param>
        /// <param name="series">The instance of the current rendered data series.</param>
        /// <param name="range">Instance of graph range.</param>
        /// <param name="xx">Collection of x coordinates.</param>
        /// <param name="yy">Collection of y coordinates.</param>
        /// <param name="minimumX">The minimum x coordinates value.</param>
        /// <param name="maximumX">The maximum x coordinates value.</param>
        /// <param name="minimumY">The minimum y coordinates value.</param>
        /// <param name="maximumY">The maximum y coordinates value.</param>
        /// <returns></returns>
        IEnumerable<PointF> Render(IGraphSurface<TDataSeries> surface, TDataSeries series, IGraphRange range, List<GraphDataPoint> xx, List<GraphDataPoint> yy, GraphDataPoint minimumX, GraphDataPoint maximumX, GraphDataPoint minimumY, GraphDataPoint maximumY);

        /// <summary>
        /// Draws the specified data series points on the target surface.
        /// </summary>
        /// <param name="surface">The target graph surface.</param>
        /// <param name="series">The instance of the current rendered data series.</param>
        /// <param name="points">The collection of the current data series drawing points.</param>
        /// <param name="index">The index of the current data series within the collection of data series.</param>
        /// <param name="count">The length of the data series collection.</param>
        void Draw(IGraphSurface<TDataSeries> surface, TDataSeries series, IEnumerable<PointF> points, int index, int count);
    }
}
