﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using RealTimeGraphX.EventArguments;
using RealTimeGraphX.Renderers;
using System.Diagnostics;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents an <see cref="IGraphController{TDataSeries, TXDataPoint, TYDataPoint}"/> base class.
    /// </summary>
    /// <typeparam name="TDataSeries">The type of the data series.</typeparam>
    /// <typeparam name="TXDataPoint">The type of the x data point.</typeparam>
    /// <typeparam name="TYDataPoint">The type of the y data point.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphObject" />
    /// <seealso cref="RealTimeGraphX.IGraphController{TDataSeries, TXDataPoint, TYDataPoint}" />
    public abstract class GraphController<TDataSeries, TXDataPoint, TYDataPoint> : GraphObject, IGraphController<TDataSeries, TXDataPoint, TYDataPoint>
        where TXDataPoint : GraphDataPoint
        where TYDataPoint : GraphDataPoint
        where TDataSeries : IGraphDataSeries
    {
        private GraphDataQueue<List<PendingSeries>> _pending_series_collection;
        private Dictionary<TDataSeries, PendingSeries> _to_render;
        private DateTime _last_render_time;
        private object _render_lock = new object();
        private Thread _render_thread;
        private bool _clear;

        #region Pending Series Class

        protected class PendingSeries
        {
            public TDataSeries Series { get; set; }
            public List<GraphDataPoint> XX { get; set; }
            public List<GraphDataPoint> YY { get; set; }

            public int NewItemsCount
            {
                get { return XX.Count - RenderedItems; }
            }

            public int RenderedItems { get; set; }

            public bool IsClearSeries { get; set; }

            public bool IsUpdateSeries { get; set; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the current effective minimum/maximum has changed.
        /// </summary>
        public event EventHandler<RangeChangedEventArgs> EffectiveRangeChanged;

        /// <summary>
        /// Occurs when the current virtual (effective minimum/maximum after transformation) minimum/maximum has changed.
        /// </summary>
        public event EventHandler<RangeChangedEventArgs> VirtualRangeChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller refresh rate.
        /// Higher rate requires more CPU time.
        /// </summary>
        public TimeSpan RefreshRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to pause rendering.
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the rendering of data.
        /// </summary>
        public bool DisableRendering { get; set; }

        /// <summary>
        /// Gets the data series collection.
        /// </summary>
        public ObservableCollection<TDataSeries> DataSeriesCollection { get; }

        private IGraphRenderer<TDataSeries> _renderer;
        /// <summary>
        /// Gets or sets the graph renderer.
        /// </summary>
        public IGraphRenderer<TDataSeries> Renderer
        {
            get
            {
                return _renderer;
            }
            set
            {
                _renderer = value; RaisePropertyChangedAuto();
            }
        }

        private IGraphSurface<TDataSeries> _surface;
        /// <summary>
        /// Gets or sets the rendering surface.
        /// </summary>
        public IGraphSurface<TDataSeries> Surface
        {
            get { return _surface; }
            set
            {
                var previous = _surface;
                _surface = value;
                RequestVirtualRangeChange();
                OnSurfaceChanged(previous, _surface);
            }
        }

        private GraphRange<TXDataPoint, TYDataPoint> _range;
        /// <summary>
        /// Gets or sets the graph range (data point boundaries).
        /// </summary>
        public GraphRange<TXDataPoint, TYDataPoint> Range
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value; RaisePropertyChangedAuto();
            }
        }

        /// <summary>
        /// Gets the current effective x-axis minimum.
        /// </summary>
        public GraphDataPoint EffectiveMinimumX { get; private set; }

        /// <summary>
        /// Gets the current effective x-axis maximum.
        /// </summary>
        public GraphDataPoint EffectiveMaximumX { get; private set; }

        /// <summary>
        /// Gets the current effective y-axis minimum.
        /// </summary>
        public GraphDataPoint EffectiveMinimumY { get; private set; }

        /// <summary>
        /// Gets the current effective y-axis maximum.
        /// </summary>
        public GraphDataPoint EffectiveMaximumY { get; private set; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) x-axis minimum.
        /// </summary>
        public GraphDataPoint VirtualMinimumX { get; private set; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) x-axis maximum.
        /// </summary>
        public GraphDataPoint VirtualMaximumX { get; private set; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) y-axis minimum.
        /// </summary>
        public GraphDataPoint VirtualMinimumY { get; private set; }

        /// <summary>
        /// Gets the current virtual (effective minimum/maximum after transformation) y-axis maximum.
        /// </summary>
        public GraphDataPoint VirtualMaximumY { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        public GraphCommand ClearCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphController{TDataSeries, TXDataPoint, TYDataPoint}"/> class.
        /// </summary>
        public GraphController()
        {
            Renderer = new ScrollingLineRenderer<TDataSeries>();

            DataSeriesCollection = new ObservableCollection<TDataSeries>();
            Range = new GraphRange<TXDataPoint, TYDataPoint>();

            _last_render_time = DateTime.Now;
            _to_render = new Dictionary<TDataSeries, PendingSeries>();
            _pending_series_collection = new GraphDataQueue<List<PendingSeries>>();
            RefreshRate = TimeSpan.FromMilliseconds(50);

            ClearCommand = new GraphCommand(Clear);

            _render_thread = new Thread(RenderThreadMethod);
            _render_thread.IsBackground = true;
            _render_thread.Priority = ThreadPriority.Highest;
            _render_thread.Start();
        }

        #endregion

        #region Render Thread

        /// <summary>
        /// The rendering thread method.
        /// </summary>
        private void RenderThreadMethod()
        {
            while (true)
            {
                if ((!IsPaused && !DisableRendering) || _clear)
                {
                    try
                    {
                        List<List<PendingSeries>> pending_lists = new List<List<PendingSeries>>();

                        var pending_list_first = _pending_series_collection.BlockDequeue();

                        if (_clear)
                        {
                            _clear = false;
                            _to_render.Clear();
                            Surface?.BeginDraw();
                            Surface?.EndDraw();

                            while (_pending_series_collection.Count > 0)
                            {
                                _pending_series_collection.BlockDequeue();
                            }
                            continue;
                        }

                        pending_lists.Add(pending_list_first);

                        while (_pending_series_collection.Count > 0)
                        {
                            var pending_list = _pending_series_collection.BlockDequeue();
                            pending_lists.Add(pending_list);
                        }

                        foreach (var pending_list in pending_lists)
                        {
                            foreach (var pending_series in pending_list)
                            {
                                if (!pending_series.IsUpdateSeries)
                                {
                                    if (_to_render.ContainsKey(pending_series.Series))
                                    {
                                        var s = _to_render[pending_series.Series];
                                        s.XX.AddRange(pending_series.XX);
                                        s.YY.AddRange(pending_series.YY);
                                    }
                                    else
                                    {
                                        _to_render[pending_series.Series] = pending_series;
                                    }
                                }
                            }
                        }

                        if (Surface != null)
                        {
                            Render();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error in RealTimeGraphX:\n{ex.ToString()}");
                    }
                }
                else if (IsPaused && !DisableRendering)
                {
                    if (Surface != null)
                    {
                        Render();
                    }
                    Thread.Sleep(RefreshRate);
                }
                else
                {
                    Thread.Sleep(RefreshRate);
                }
            }
        }

        private void Render()
        {
            if (_to_render.Count > 0)
            {
                GraphDataPoint min_x = _range.MaximumX - _range.MaximumX;
                GraphDataPoint max_x = _range.MaximumX;
                GraphDataPoint min_y = _range.MinimumY;
                GraphDataPoint max_y = _range.MaximumY;

                if (_to_render.Count > 0 && _to_render.First().Value.XX.Count > 0)
                {
                    min_x = _to_render.First().Value.XX.First();
                    max_x = _to_render.First().Value.XX.Last();

                    if ( ( max_x - min_x ) < _range.MaximumX )
                    {
                        switch ( _range.XStartBehavior )
                        {
                            case XStartBehavior.stretchData:
                                // Original behavior. No action necessary.
                                break ;
                            case XStartBehavior.slideInFromRight:
                                min_x = max_x - _range.MaximumX ;
                                break ;
                            case XStartBehavior.fillFromLeft:
                                max_x = min_x + _range.MaximumX ;
                                break ;
                        }
                    }
                }
                else
                {
                    return;
                }

                if (_range.AutoY)
                {
                    min_y = _to_render.Select(x => x.Value).SelectMany(x => x.YY).Min();
                    max_y = _to_render.Select(x => x.Value).SelectMany(x => x.YY).Max();

                    // Note: The "this" object to which this method is applied is not used,
                    // making it like a static method (but abstract). I'm not really happy
                    // with this structure.
                    min_y.ExpandRange ( ref min_y, ref max_y, 0.05 ) ;
                }

                if (min_y == max_y)
                {
                    if (_range.AutoYFallbackMode == GraphRangeAutoYFallBackMode.MinMax)
                    {
                        min_y = _range.MinimumY;
                        max_y = _range.MaximumY;
                    }
                    else if (_range.AutoYFallbackMode == GraphRangeAutoYFallBackMode.Margins)
                    {
                        min_y -= _range.AutoYFallbackMargins;
                        max_y += _range.AutoYFallbackMargins;
                    }
                }

                EffectiveMinimumX = min_x;
                EffectiveMaximumX = max_x;
                EffectiveMinimumY = min_y;
                EffectiveMaximumY = max_y;

                VirtualMinimumX = EffectiveMinimumX;
                VirtualMaximumX = EffectiveMaximumX;
                VirtualMinimumY = EffectiveMinimumY;
                VirtualMaximumY = EffectiveMaximumY;

                _last_render_time = DateTime.Now;

                if (Surface != null)
                {
                    var surface_size = Surface.GetSize();
                    var zoom_rect = Surface.GetZoomRect();

                    if (surface_size.Width > 0 && surface_size.Height > 0)
                    {
                        Surface.BeginDraw();

                        if (zoom_rect.Width > 0 && zoom_rect.Height > 0)
                        {
                            var zoom_rect_top_percentage = zoom_rect.Top / surface_size.Height;
                            var zoom_rect_bottom_percentage = zoom_rect.Bottom / surface_size.Height;
                            var zoom_rect_left_percentage = zoom_rect.Left / surface_size.Width;
                            var zoom_rect_right_percentage = zoom_rect.Right / surface_size.Width;

                            VirtualMinimumY = EffectiveMaximumY - GraphDataPointHelper.ComputeAbsolutePosition(EffectiveMinimumY, EffectiveMaximumY, zoom_rect_bottom_percentage);
                            VirtualMaximumY = EffectiveMaximumY - GraphDataPointHelper.ComputeAbsolutePosition(EffectiveMinimumY, EffectiveMaximumY, zoom_rect_top_percentage);

                            VirtualMinimumX = GraphDataPointHelper.ComputeAbsolutePosition(EffectiveMinimumX, EffectiveMaximumX, zoom_rect_left_percentage);
                            VirtualMaximumX = GraphDataPointHelper.ComputeAbsolutePosition(EffectiveMinimumX, EffectiveMaximumX, zoom_rect_right_percentage);

                            GraphTransform transform = new GraphTransform();
                            var scale_x = (float)(surface_size.Width / zoom_rect.Width);
                            var scale_y = (float)(surface_size.Height / zoom_rect.Height);
                            var translate_x = (float)-zoom_rect.Left * scale_x;
                            var translate_y = (float)-zoom_rect.Top * scale_y;

                            transform = new GraphTransform();
                            transform.TranslateX = translate_x;
                            transform.TranslateY = translate_y;
                            transform.ScaleX = scale_x;
                            transform.ScaleY = scale_y;

                            Surface.SetTransform(transform);
                        }

                        List<Tuple<TDataSeries, IEnumerable<PointF>>> to_draw = new List<Tuple<TDataSeries, IEnumerable<PointF>>>();

                        var to_render = _to_render.Select(x => x.Value).ToList();

                        foreach (var item in to_render)
                        {
                            if (item.YY.Count > 0)
                            {
                                item.Series.CurrentValue = item.YY.Last().GetValue();
                            }

                            var points = Renderer.Render(Surface, item.Series, _range, item.XX, item.YY, min_x, max_x, min_y, max_y);
                            to_draw.Add(new Tuple<TDataSeries, IEnumerable<PointF>>(item.Series, points));
                        }

                        for (int i = 0; i < to_draw.Count; i++)
                        {
                            if (to_draw[i].Item2.Count() > 2)
                            {
                                if (to_draw[i].Item1.IsVisible)
                                {
                                    Renderer.Draw(Surface, to_draw[i].Item1, to_draw[i].Item2, i, to_draw.Count);
                                }
                            }
                        }

                        Surface?.EndDraw();
                    }
                }

                OnEffectiveRangeChanged(EffectiveMinimumX, EffectiveMaximumX, EffectiveMinimumY, EffectiveMaximumY);
                OnVirtualRangeChanged(VirtualMinimumX, VirtualMaximumX, VirtualMinimumY, VirtualMaximumY);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when the surface has changed.
        /// </summary>
        /// <param name="previous">The previous.</param>
        /// <param name="surface">The surface.</param>
        protected virtual void OnSurfaceChanged(IGraphSurface<TDataSeries> previous, IGraphSurface<TDataSeries> surface)
        {
            if (previous != null)
            {
                previous.SurfaceSizeChanged -= Surface_SurfaceSizeChanged;
                previous.ZoomRectChanged -= Surface_ZoomRectChanged;
            }

            if (surface != null)
            {
                surface.SurfaceSizeChanged += Surface_SurfaceSizeChanged;
                surface.ZoomRectChanged += Surface_ZoomRectChanged;
            }
        }

        /// <summary>
        /// Raises the <see cref="EffectiveRangeChanged"/> event.
        /// </summary>
        /// <param name="minimumX">The minimum x.</param>
        /// <param name="maximumX">The maximum x.</param>
        /// <param name="minimumY">The minimum y.</param>
        /// <param name="maximumY">The maximum y.</param>
        protected virtual void OnEffectiveRangeChanged(GraphDataPoint minimumX, GraphDataPoint maximumX, GraphDataPoint minimumY, GraphDataPoint maximumY)
        {
            EffectiveRangeChanged?.Invoke(this, new RangeChangedEventArgs(minimumX, maximumX, minimumY, maximumY));
        }

        /// <summary>
        /// Raises the <see cref="VirtualRangeChanged"/> event.
        /// </summary>
        /// <param name="minimumX">The minimum x.</param>
        /// <param name="maximumX">The maximum x.</param>
        /// <param name="minimumY">The minimum y.</param>
        /// <param name="maximumY">The maximum y.</param>
        protected virtual void OnVirtualRangeChanged(GraphDataPoint minimumX, GraphDataPoint maximumX, GraphDataPoint minimumY, GraphDataPoint maximumY)
        {
            var range = new RangeChangedEventArgs(minimumX, maximumX, minimumY, maximumY);
            VirtualRangeChanged?.Invoke(this, range);
        }

        /// <summary>
        /// Converts the specified relative x position to graph absolute position.
        /// </summary>
        /// <param name="x">The relative x position.</param>
        /// <returns></returns>
        protected virtual float ConvertXValueToRendererValue(double x)
        {
            return (float)(x * Surface.GetSize().Width / 100);
        }

        /// <summary>
        /// Converts the specified relative y position to graph absolute position.
        /// </summary>
        /// <param name="y">The relative y position.</param>
        /// <returns></returns>
        protected virtual float ConvertYValueToRendererValue(double y)
        {
            return (float)(Surface.GetSize().Height - (y * Surface.GetSize().Height / 100));
        }

        #endregion

        #region Surface Event Handlers

        /// <summary>
        /// Handles the ZoomRectChanged event of the Surface control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Surface_ZoomRectChanged(object sender, EventArgs e)
        {
            if (!_pending_series_collection.ToList().SelectMany(x => x).ToList().Exists(x => x.IsUpdateSeries))
            {
                List<PendingSeries> updateSeries = new List<PendingSeries>();

                foreach (var pending_Series in _to_render)
                {
                    updateSeries.Add(new PendingSeries()
                    {
                        IsUpdateSeries = true,
                        Series = pending_Series.Value.Series,
                        XX = new List<GraphDataPoint>(),
                        YY = new List<GraphDataPoint>(),
                    });
                }

                _pending_series_collection.BlockEnqueue(updateSeries);
            }
        }

        /// <summary>
        /// Handles the SurfaceSizeChanged event of the Surface control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Surface_SurfaceSizeChanged(object sender, EventArgs e)
        {
            if (!_pending_series_collection.ToList().SelectMany(x => x).ToList().Exists(x => x.IsUpdateSeries))
            {
                List<PendingSeries> updateSeries = new List<PendingSeries>();

                foreach (var pending_Series in _to_render)
                {
                    updateSeries.Add(new PendingSeries()
                    {
                        IsUpdateSeries = true,
                        Series = pending_Series.Value.Series,
                        XX = new List<GraphDataPoint>(),
                        YY = new List<GraphDataPoint>(),
                    });
                }

                _pending_series_collection.BlockEnqueue(updateSeries);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Submits the specified x and y data points.
        /// If the controller has more than one data series the data points will be duplicated.
        /// </summary>
        /// <param name="x">X data point.</param>
        /// <param name="y">Y data point.</param>
        public void PushData(TXDataPoint x, TYDataPoint y)
        {
            if (DataSeriesCollection.Count == 0) return;

            List<List<TXDataPoint>> xxxx = new List<List<TXDataPoint>>();
            List<List<TYDataPoint>> yyyy = new List<List<TYDataPoint>>();

            foreach (var series in DataSeriesCollection.ToList())
            {
                xxxx.Add(new List<TXDataPoint>() { x });
                yyyy.Add(new List<TYDataPoint>() { y });
            }

            PushData(xxxx, yyyy);
        }

        /// <summary>
        /// Submits the specified collections of x and y data points.
        /// If the controller has more than one data series the data points will be distributed evenly.
        /// </summary>
        /// <param name="xx">X data point collection.</param>
        /// <param name="yy">Y data point collection.</param>
        public void PushData(IEnumerable<TXDataPoint> xx, IEnumerable<TYDataPoint> yy)
        {
            if (DataSeriesCollection.Count == 0) return;

            var xList = xx.ToList();
            var yList = yy.ToList();

            List<List<TXDataPoint>> xxxx = new List<List<TXDataPoint>>();
            List<List<TYDataPoint>> yyyy = new List<List<TYDataPoint>>();

            foreach (var series in DataSeriesCollection.ToList())
            {
                xxxx.Add(new List<TXDataPoint>());
                yyyy.Add(new List<TYDataPoint>());
            }

            int counter = 0;

            for (int i = 0; i < xList.Count; i++)
            {
                xxxx[counter].Add(xList[i]);
                yyyy[counter].Add(yList[i]);

                counter++;

                if (counter >= xxxx.Count)
                {
                    counter = 0;
                }
            }

            PushData(xxxx, yyyy);
        }

        /// <summary>
        /// Submits a matrix of x and y data points. Meaning each data series should process a single collection of x/y data points.
        /// </summary>
        /// <param name="xxxx">X matrix.</param>
        /// <param name="yyyy">Y matrix.</param>
        public void PushData(IEnumerable<IEnumerable<TXDataPoint>> xxxx, IEnumerable<IEnumerable<TYDataPoint>> yyyy)
        {
            if (DataSeriesCollection.Count == 0) return;

            IEnumerable<IEnumerable<GraphDataPoint>> xxxxI = xxxx.Select(x => x.ToList()).ToList();
            IEnumerable<IEnumerable<GraphDataPoint>> yyyyI = yyyy.Select(x => x.ToList()).ToList();

            List<List<GraphDataPoint>> xxxxList = xxxxI.Select(x => x.ToList()).ToList();
            List<List<GraphDataPoint>> yyyyList = yyyyI.Select(x => x.ToList()).ToList();

            int first_count_x = xxxxList[0].Count;
            int first_count_y = yyyyList[0].Count;


            bool is_data_valid = true;

            for (int i = 0; i < xxxxList.Count; i++)
            {
                if (xxxxList[0].Count != first_count_x)
                {
                    is_data_valid = false;
                    break;
                }

                if (xxxxList[0].Count != yyyyList[0].Count)
                {
                    is_data_valid = false;
                    break;
                }
            }

            if (!is_data_valid)
            {
                throw new ArgumentOutOfRangeException("When pushing data to a multi series renderer, each series must contain the same amount of data.");
            }

            var list = DataSeriesCollection.ToList();

            var pending_list = new List<PendingSeries>();

            for (int i = 0; i < list.Count; i++)
            {
                pending_list.Add(new PendingSeries()
                {
                    Series = list[i],
                    XX = xxxxList[i].ToList(),
                    YY = yyyyList[i].ToList(),
                });
            }

            _pending_series_collection.BlockEnqueue(pending_list);
        }

        /// <summary>
        /// Clears all data points from this controller.
        /// </summary>
        public void Clear()
        {
            _clear = true;

            _pending_series_collection.BlockEnqueue(new List<PendingSeries>()
            {
                  new PendingSeries()
                  {
                      IsClearSeries = true
                  },
            });
        }

        /// <summary>
        /// Requests the controller to invoke a virtual range change event.
        /// </summary>
        public void RequestVirtualRangeChange()
        {
            OnVirtualRangeChanged(Range.MaximumX, Range.MaximumX, Range.MinimumY, Range.MaximumY);
        }

        /// <summary>
        /// Translates a surface x position to graph value.
        /// </summary>
        /// <param name="surfaceXPosition">The surface x position.</param>
        /// <returns></returns>
        public IGraphDataPoint TranslateSurfaceX(double surfaceXPosition)
        {
            if (Surface != null)
            {
                var relativeX = surfaceXPosition / Surface.GetSize().Width;
                return GraphDataPointHelper.ComputeAbsolutePosition(VirtualMinimumX, VirtualMaximumX, relativeX);
            }
            else
            {
                return default(IGraphDataPoint);
            }
        }

        /// <summary>
        /// Translates a surface y position to graph value.
        /// </summary>
        /// <param name="surfaceYPosition">The surface y position.</param>
        /// <returns></returns>
        public IGraphDataPoint TranslateSurfaceY(double surfaceYPosition)
        {
            if (Surface != null)
            {
                var relativeY = 1 - surfaceYPosition / Surface.GetSize().Height;
                return GraphDataPointHelper.ComputeAbsolutePosition(VirtualMinimumY, VirtualMaximumY, relativeY);
            }
            else
            {
                return default(IGraphDataPoint);
            }
        }

        #endregion
    }
}
