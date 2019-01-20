using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RealTimeGraphX.Renderers
{
    /// <summary>
    /// Represents a scrolling style <see cref="GraphRenderer{TDataSeries}"/>.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphRenderer{TDataSeries}" />
    public class ScrollingLineRenderer<TDataSeries> : GraphRenderer<TDataSeries> where TDataSeries : IGraphDataSeries
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
        public override IEnumerable<PointF> Render(IGraphSurface<TDataSeries> surface, TDataSeries series, IGraphRange range, List<GraphDataPoint> xx, List<GraphDataPoint> yy, GraphDataPoint minimumX, GraphDataPoint maximumX, GraphDataPoint minimumY, GraphDataPoint maximumY)
        {
            var dxList = xx.Select(x => x.ComputeRelativePosition(minimumX, maximumX)).ToList();
            var dyList = yy.Select(x => x.ComputeRelativePosition(minimumY, maximumY)).ToList();

            if (maximumX - minimumX > range.MaximumX)
            {
                var offset = ((maximumX - minimumX) - range.MaximumX) + minimumX;

                for (int i = 0; i < xx.Count; i++)
                {
                    if (xx[i] < offset)
                    {
                        xx.RemoveAt(i);
                        yy.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            List<PointF> points = new List<PointF>();

            for (int i = 0; i < dxList.Count; i++)
            {
                float image_x = ConvertXValueToRendererValue(surface,dxList[i]);
                float image_y = (float)Math.Min(ConvertYValueToRendererValue(surface, dyList[i]), surface.GetSize().Height - 2);

                PointF point = new PointF(image_x, image_y);
                points.Add(point);
            }

            return points;
        }

        /// <summary>
        /// Draws the specified data series points on the target surface.
        /// </summary>
        /// <param name="surface">The target graph surface.</param>
        /// <param name="series">The instance of the current rendered data series.</param>
        /// <param name="points">The collection of the current data series drawing points.</param>
        /// <param name="index">The index of the current data series within the collection of data series.</param>
        /// <param name="count">The length of the data series collection.</param>
        public override void Draw(IGraphSurface<TDataSeries> surface, TDataSeries series, IEnumerable<PointF> points, int index, int count)
        {
            if (series.UseFill)
            {
                surface.FillSeries(series, GetFillPoints(surface, points));
            }

            surface.DrawSeries(series, points);
        }
    }
}
