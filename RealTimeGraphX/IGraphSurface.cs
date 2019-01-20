using RealTimeGraphX.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents a graph drawing surface capable of drawing a series of points submitted by a graph renderer.
    /// </summary>
    public interface IGraphSurface : IGraphComponent
    {
        /// <summary>
        /// Returns the actual size of the surface.
        /// </summary>
        /// <returns></returns>
        SizeF GetSize();

        /// <summary>
        /// Returns the current bounds of the zooming rectangle.
        /// </summary>
        /// <returns></returns>
        RectangleF GetZoomRect();
    }

    /// <summary>
    /// Represents a graph drawing surface capable of drawing a series of points submitted by a graph renderer.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <seealso cref="RealTimeGraphX.IGraphComponent" />
    public interface IGraphSurface<TDataSeries> : IGraphSurface where TDataSeries : IGraphDataSeries
    {
        /// <summary>
        /// Called before drawing has started.
        /// </summary>
        void BeginDraw();

        /// <summary>
        /// Draws the stroke of the specified data series points.
        /// </summary>
        /// <param name="dataSeries">The data series.</param>
        /// <param name="points">The points.</param>
        void DrawSeries(TDataSeries dataSeries, IEnumerable<PointF> points);

        /// <summary>
        /// Fills the specified data series points.
        /// </summary>
        /// <param name="dataSeries">The data series.</param>
        /// <param name="points">The points.</param>
        void FillSeries(TDataSeries dataSeries, IEnumerable<PointF> points);

        /// <summary>
        /// Applies transformation on the current pass.
        /// </summary>
        /// <param name="transform">The transform.</param>
        void SetTransform(GraphTransform transform);

        /// <summary>
        /// Called when drawing has completed.
        /// </summary>
        void EndDraw();
    }
}
