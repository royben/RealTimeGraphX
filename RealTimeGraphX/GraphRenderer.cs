using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents an <see cref="IGraphRenderer{TDataSeries}"/> base class.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphObject" />
    /// <seealso cref="RealTimeGraphX.IGraphRenderer{TDataSeries}" />
    public abstract class GraphRenderer<TDataSeries> : GraphObject, IGraphRenderer<TDataSeries> where TDataSeries : IGraphDataSeries
    {
        /// <summary>
        /// Converts the specified relative x position to a graph surface absolute position.
        /// </summary>
        /// <param name="surface">The target surface.</param>
        /// <param name="x">The relative x position.</param>
        /// <returns></returns>
        protected virtual float ConvertXValueToRendererValue(IGraphSurface<TDataSeries> surface, double x)
        {
            return (float)(x * surface.GetSize().Width / 100);
        }

        /// <summary>
        /// Converts the specified relative y position to a graph surface absolute position.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="y">The relative y position.</param>
        /// <returns></returns>
        protected virtual float ConvertYValueToRendererValue(IGraphSurface<TDataSeries> surface, double y)
        {
            return (float)(surface.GetSize().Height - (y * surface.GetSize().Height / 100));
        }

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
        public abstract IEnumerable<PointF> Render(IGraphSurface<TDataSeries> surface, TDataSeries series, IGraphRange range, List<GraphDataPoint> xx, List<GraphDataPoint> yy, GraphDataPoint minimumX, GraphDataPoint maximumX, GraphDataPoint minimumY, GraphDataPoint maximumY);

        /// <summary>
        /// Draws the specified data series points on the target surface.
        /// </summary>
        /// <param name="surface">The target graph surface.</param>
        /// <param name="series">The instance of the current rendered data series.</param>
        /// <param name="points">The collection of the current data series drawing points.</param>
        /// <param name="index">The index of the current data series within the collection of data series.</param>
        /// <param name="count">The length of the data series collection.</param>
        public abstract void Draw(IGraphSurface<TDataSeries> surface, TDataSeries series, IEnumerable<PointF> points, int index, int count);

        /// <summary>
        /// Gets a closed polygon version of the specified drawing points.
        /// </summary>
        /// <param name="surface">The target surface.</param>
        /// <param name="points">The drawing points.</param>
        /// <returns></returns>
        protected virtual IEnumerable<PointF> GetFillPoints(IGraphSurface<TDataSeries> surface, IEnumerable<PointF> points)
        {
            List<PointF> closed = points.ToList();
            closed.Add(new PointF(points.Last().X, surface.GetSize().Width));
            closed.Add(new PointF(0, surface.GetSize().Height));
            return closed.ToArray();
        }
    }
}
