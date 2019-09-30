using RealTimeGraphX.EventArguments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents a graph controller.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.IGraphComponent" />
    public interface IGraphController : IGraphComponent
    {
        #region Events

        /// <summary>
        /// Occurs when the current effective minimum/maximum values have changed.
        /// </summary>
        event EventHandler<RangeChangedEventArgs> EffectiveRangeChanged;

        /// <summary>
        /// Occurs when the current virtual (effective minimum/maximum after transformation) minimum/maximum values have changed.
        /// </summary>
        event EventHandler<RangeChangedEventArgs> VirtualRangeChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller refresh rate.
        /// Higher rate requires more CPU time.
        /// </summary>
        TimeSpan RefreshRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to pause the rendering.
        /// </summary>
        bool IsPaused { get; set; }

        /// <summary>
        /// Gets the current effective x-axis minimum.
        /// </summary>
        GraphDataPoint EffectiveMinimumX { get; }

        /// <summary>
        /// Gets the current effective x-axis maximum.
        /// </summary>
        GraphDataPoint EffectiveMaximumX { get; }

        /// <summary>
        /// Gets the current effective y-axis minimum.
        /// </summary>
        GraphDataPoint EffectiveMinimumY { get; }

        /// <summary>
        /// Gets the current effective y-axis maximum.
        /// </summary>
        GraphDataPoint EffectiveMaximumY { get; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) x-axis minimum.
        /// </summary>
        GraphDataPoint VirtualMinimumX { get; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) x-axis maximum.
        /// </summary>
        GraphDataPoint VirtualMaximumX { get; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) y-axis minimum.
        /// </summary>
        GraphDataPoint VirtualMinimumY { get; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) y-axis maximum.
        /// </summary>
        GraphDataPoint VirtualMaximumY { get; }

        /// <summary>
        /// Clears all data points from this controller.
        /// </summary>
        void Clear();

        #endregion

        #region Commands

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        GraphCommand ClearCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Requests the controller to invoke a virtual range change event.
        /// </summary>
        void RequestVirtualRangeChange();

        #endregion
    }

    /// <summary>
    /// Represents a graph controller capable of pushing data points to it's associated <see cref="IGraphRenderer{TDataSeries}">Graph Renderer</see>
    /// and rendering them to it's associated <see cref="IGraphSurface{TDataSeries}">Graph Surface</see>.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <seealso cref="RealTimeGraphX.IGraphComponent" />
    public interface IGraphController<TDataSeries> : IGraphController where TDataSeries : IGraphDataSeries
    {
        #region Properties

        /// <summary>
        /// Gets the data series collection.
        /// </summary>
        ObservableCollection<TDataSeries> DataSeriesCollection { get; }

        /// <summary>
        /// Gets or sets the graph renderer.
        /// </summary>
        IGraphRenderer<TDataSeries> Renderer { get; set; }

        /// <summary>
        /// Gets or sets the graph surface.
        /// </summary>
        IGraphSurface<TDataSeries> Surface { get; set; }

        #endregion
    }


    /// <summary>
    /// Represents a graph controller capable of pushing data points to it's associated <see cref="IGraphRenderer{TDataSeries}">Graph Renderer</see>
    /// and rendering them to it's associated <see cref="IGraphSurface{TDataSeries}">Graph Surface</see>.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <typeparam name="TXDataPoint">The type of the x data point.</typeparam>
    /// <typeparam name="TYDataPoint">The type of the y data point.</typeparam>
    /// <seealso cref="RealTimeGraphX.IGraphComponent" />
    public interface IGraphController<TDataSeries, TXDataPoint, TYDataPoint> : IGraphController<TDataSeries>
        where TXDataPoint : GraphDataPoint
        where TYDataPoint : GraphDataPoint
        where TDataSeries : IGraphDataSeries
    {
        #region Properties

        /// <summary>
        /// Gets or sets the graph range (data point boundaries).
        /// </summary>
        GraphRange<TXDataPoint, TYDataPoint> Range { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Submits the specified x and y data points.
        /// If the controller has more than one data series the data points will be duplicated.
        /// </summary>
        /// <param name="x">X data point.</param>
        /// <param name="y">Y data point.</param>
        void PushData(TXDataPoint x, TYDataPoint y);

        /// <summary>
        /// Submits the specified collections of x and y data points.
        /// If the controller has more than one data series the data points will be distributed evenly. 
        /// </summary>
        /// <param name="xx">X data point collection.</param>
        /// <param name="yy">Y data point collection.</param>
        void PushData(IEnumerable<TXDataPoint> xx, IEnumerable<TYDataPoint> yy);

        /// <summary>
        /// Submits a matrix of x and y data points. Meaning each data series should process a single collection of x/y data points.
        /// </summary>
        /// <param name="xxxx">X matrix.</param>
        /// <param name="yyyy">Y matrix.</param>
        void PushData(IEnumerable<IEnumerable<TXDataPoint>> xxxx, IEnumerable<IEnumerable<TYDataPoint>> yyyy);

        #endregion
    }
}
