using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX
{
    /// <summary>
    ///  Represents a graph x/y data points boundaries.
    /// </summary>
    public interface IGraphRange : IGraphComponent
    {
        /// <summary>
        /// Gets or sets the maximum x value.
        /// </summary>
        GraphDataPoint MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum x value.
        /// </summary>
        GraphDataPoint MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the maximum y value.
        /// </summary>
        GraphDataPoint MaximumY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically adjust the graph <see cref="MaximumY"/> and <see cref="MinimumY"/> according to the current effective data points.
        /// </summary>
        bool AutoY { get; set; }

        /// <summary>
        /// Gets or sets the fallback mode for when AutoY is set to true and all Y values are equal so to retain a perspective.
        /// </summary>
        GraphRangeAutoYFallBackMode AutoYFallbackMode { get; set; }

        /// <summary>
        /// Gets or sets the AutoY fallback margins when <see cref="AutoYFallbackMode"/> is set to Margins.
        /// </summary>
        GraphDataPoint AutoYFallBackMargins { get; set; }
    }

    /// <summary>
    /// Represents a graph X/Y range boundaries.
    /// </summary>
    /// <typeparam name="XDataPoint">Type of x-axis data point.</typeparam>
    /// <typeparam name="YDataPoint">Type of y-axis data point.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphObject" />
    public class GraphRange<XDataPoint, YDataPoint> : GraphObject, IGraphRange where XDataPoint : GraphDataPoint where YDataPoint : GraphDataPoint
    {
        private XDataPoint _maximumX;
        /// <summary>
        /// Gets or sets the maximum x value.
        /// </summary>
        public XDataPoint MaximumX
        {
            get { return _maximumX; }
            set { _maximumX = value; RaisePropertyChangedAuto(); }
        }

        private YDataPoint _minimumY;
        /// <summary>
        /// Gets or sets the minimum x value.
        /// </summary>
        public YDataPoint MinimumY
        {
            get { return _minimumY; }
            set { _minimumY = value; RaisePropertyChangedAuto(); }
        }

        private YDataPoint _maximumY;
        /// <summary>
        /// Gets or sets the maximum y value.
        /// </summary>
        public YDataPoint MaximumY
        {
            get { return _maximumY; }
            set { _maximumY = value; RaisePropertyChangedAuto(); }
        }

        private bool _autoY;
        /// <summary>
        /// Gets or sets a value indicating whether to automatically adjust the graph <see cref="MaximumY"/> and <see cref="MinimumY"/> according to the current visible data points.
        /// </summary>
        public bool AutoY
        {
            get { return _autoY; }
            set { _autoY = value; RaisePropertyChangedAuto(); }
        }

        private GraphRangeAutoYFallBackMode _autoYFallbackMode;
        /// <summary>
        /// Gets or sets the fallback mode for when AutoY is set to true and all Y values are equal so to retain a perspective.
        /// </summary>
        public GraphRangeAutoYFallBackMode AutoYFallbackMode
        {
            get { return _autoYFallbackMode; }
            set { _autoYFallbackMode = value; RaisePropertyChangedAuto(); }
        }

        private YDataPoint _autoYFallbackMargins;
        /// <summary>
        /// Gets or sets the margins for when <see cref="AutoYFallbackMode"/> is set to <see cref="GraphRangeAutoYFallBackMode.Margins"/>.
        /// </summary>
        public YDataPoint AutoYFallbackMargins
        {
            get { return _autoYFallbackMargins; }
            set { _autoYFallbackMargins = value; RaisePropertyChangedAuto(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphRange{XDataPoint, YDataPoint}"/> class.
        /// </summary>
        public GraphRange()
        {
            MinimumY = GraphDataPointHelper.Init<YDataPoint>();
            MaximumX = GraphDataPointHelper.Init<XDataPoint>();
            MaximumY = GraphDataPointHelper.Init<YDataPoint>();
            AutoYFallbackMode = GraphRangeAutoYFallBackMode.Margins;
            AutoYFallbackMargins = GraphDataPointHelper.GetDefaultMargins<YDataPoint>();
        }

        /// <summary>
        /// Gets or sets the maximum x value.
        /// </summary>
        GraphDataPoint IGraphRange.MaximumX
        {
            get
            {
                return MaximumX;
            }
            set
            {
                MaximumX = (XDataPoint)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum x value.
        /// </summary>
        GraphDataPoint IGraphRange.MinimumY
        {
            get
            {
                return MinimumY;
            }
            set
            {
                MinimumY = (YDataPoint)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum y value.
        /// </summary>
        GraphDataPoint IGraphRange.MaximumY
        {
            get
            {
                return MaximumY;
            }
            set
            {
                MaximumY = (YDataPoint)value;
            }
        }

        /// <summary>
        /// Gets or sets the fallback mode for when AutoY is set to true and all Y values are equal so to retain a perspective.
        /// </summary>
        GraphDataPoint IGraphRange.AutoYFallBackMargins
        {
            get
            {
                return AutoYFallbackMargins;
            }
            set
            {
                AutoYFallbackMargins = (YDataPoint)value;
            }
        }
    }
}
