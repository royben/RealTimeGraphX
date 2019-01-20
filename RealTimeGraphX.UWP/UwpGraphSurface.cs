using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using RealTimeGraphX.EventArguments;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents a UWP <see cref="IGraphSurface{TDataSeries}">graph surface</see>.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    /// <seealso cref="RealTimeGraphX.IGraphSurface{RealTimeGraphX.UWP.UwpGraphDataSeries}" />
    public class UwpGraphSurface : Control, IGraphSurface<UwpGraphDataSeries>
    {
        private CanvasRenderTarget _bitmap;
        private System.Drawing.SizeF _size;
        private System.Drawing.RectangleF _zoom_rect;
        private bool _size_changed;
        private CanvasDrawingSession _session;
        private CanvasControl _canvas2d;
        private Windows.UI.Xaml.Shapes.Rectangle _selection_rectangle;
        private Canvas _selection_canvas;
        private bool _is_selection_mouse_down_zoom;
        private bool _is_selection_mouse_down_pan;
        private Point _selection_start_point;
        private bool _is_scaled;
        private Point _current_mouse_position;
        private Point _last_mouse_position;

        #region Properties

        /// <summary>
        /// Gets or sets the graph controller.
        /// </summary>
        public IGraphController<UwpGraphDataSeries> Controller
        {
            get { return (IGraphController<UwpGraphDataSeries>)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IGraphController<UwpGraphDataSeries>), typeof(UwpGraphSurface), new PropertyMetadata(null, (d, e) => (d as UwpGraphSurface).OnControllerChanged(e.OldValue as IGraphController<UwpGraphDataSeries>, e.NewValue as IGraphController<UwpGraphDataSeries>)));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UwpGraphSurface"/> class.
        /// </summary>
        public UwpGraphSurface()
        {
            this.DefaultStyleKey = typeof(UwpGraphSurface);
            SizeChanged += GraphSurface_SizeChanged;
        }

        #endregion

        #region Apply Template

        /// <summary>
        /// Handles the On Apply Template event.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvas2d = GetTemplateChild("PART_Canvas2D") as CanvasControl;
            _selection_rectangle = GetTemplateChild("PART_SelectionRectangle") as Windows.UI.Xaml.Shapes.Rectangle;
            _selection_canvas = GetTemplateChild("PART_SelectionCanvas") as Canvas;
            _canvas2d.Draw += OnCanvasDraw;

            _selection_canvas.PointerPressed += OnSelectionCanvasPointerPressed;
            _selection_canvas.PointerReleased += OnSelectionCanvasPointerReleased;
            _selection_canvas.PointerMoved += OnSelectionCanvasPointerMoved;
            _selection_canvas.DoubleTapped += OnSelectionCanvasDoubleTapped;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when the mouse moves over the zoom/pan selection canvas.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSelectionCanvasPointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _current_mouse_position = e.GetCurrentPoint(_selection_canvas).Position;

            if (_is_selection_mouse_down_zoom && IsCtrlDown())
            {
                Canvas.SetLeft(_selection_rectangle, _selection_start_point.X);
                Canvas.SetTop(_selection_rectangle, _selection_start_point.Y);

                Point _selection_current_point = _current_mouse_position;

                if (_selection_current_point.X - _selection_start_point.X > 1)
                {
                    _selection_rectangle.Width = _selection_current_point.X - _selection_start_point.X;
                }

                if (_selection_current_point.Y - _selection_start_point.Y > 1)
                {
                    _selection_rectangle.Height = _selection_current_point.Y - _selection_start_point.Y;
                }

                if (_selection_current_point.X < _selection_start_point.X)
                {
                    Canvas.SetLeft(_selection_rectangle, _selection_current_point.X);
                    _selection_rectangle.Width = _selection_start_point.X - _selection_current_point.X;
                }

                if (_selection_current_point.Y < _selection_start_point.Y)
                {
                    Canvas.SetTop(_selection_rectangle, _selection_current_point.Y);
                    _selection_rectangle.Height = _selection_start_point.Y - _selection_current_point.Y;
                }
            }
            else if (_is_selection_mouse_down_pan && _is_scaled)
            {
                Point _selection_current_point = _current_mouse_position;

                double delta_x = _current_mouse_position.X - _last_mouse_position.X;
                double delta_y = _current_mouse_position.Y - _last_mouse_position.Y;

                double x = _zoom_rect.Left - delta_x;
                double y = _zoom_rect.Top - delta_y;

                if (x < 0)
                {
                    x = 0;
                }

                if (y < 0)
                {
                    y = 0;
                }

                if (x + _zoom_rect.Width > _size.Width)
                {
                    x = x - (x + _zoom_rect.Width - _size.Width);
                }

                if (y + _zoom_rect.Height > _size.Height)
                {
                    y = y - (y + _zoom_rect.Height - _size.Height);
                }

                _zoom_rect = new System.Drawing.RectangleF((float)x, (float)y, _zoom_rect.Width, _zoom_rect.Height);
            }

            _last_mouse_position = _current_mouse_position;
        }

        /// <summary>
        /// Called when the mouse released from the zoom/pan selection canvas.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSelectionCanvasPointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _selection_canvas.ReleasePointerCapture(e.Pointer);

            if (_is_selection_mouse_down_pan)
            {
                _is_selection_mouse_down_pan = false;
            }
            else if (_is_selection_mouse_down_zoom)
            {
                _is_selection_mouse_down_zoom = false;

                _zoom_rect = new System.Drawing.RectangleF((float)Canvas.GetLeft(_selection_rectangle), (float)Canvas.GetTop(_selection_rectangle), (float)_selection_rectangle.Width, (float)_selection_rectangle.Height);
                _selection_rectangle.Visibility = Visibility.Collapsed;
                _is_scaled = true;
            }
        }

        /// <summary>
        /// Called when the mouse pressed on the zoom/pan selection canvas.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSelectionCanvasPointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _selection_canvas.CapturePointer(e.Pointer);

            _selection_start_point = e.GetCurrentPoint(_selection_canvas).Position;
            _current_mouse_position = _selection_start_point;
            _last_mouse_position = _current_mouse_position;

            if (IsCtrlDown())
            {
                _selection_rectangle.Width = 0;
                _selection_rectangle.Height = 0;
                _is_selection_mouse_down_zoom = true;
                _is_selection_mouse_down_pan = false;
                _selection_rectangle.Visibility = Visibility.Visible;
            }
            else
            {
                _is_selection_mouse_down_pan = true;
            }
        }

        /// <summary>
        /// Handles the On Selection Canvas Double Tapped event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSelectionCanvasDoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            _zoom_rect = new System.Drawing.RectangleF();
            _is_scaled = false;
        }

        /// <summary>
        /// Draws the current session.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CanvasDrawEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCanvasDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_bitmap != null)
            {
                args.DrawingSession.DrawImage(_bitmap);
            }
        }

        /// <summary>
        /// Determines whether the ctrl key is pressed.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsCtrlDown()
        {
            var state = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.LeftControl);
            return ((state & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down);
        }

        /// <summary>
        /// Called when the <see cref="Controller"/> has changed.
        /// </summary>
        /// <param name="oldController">The old controller.</param>
        /// <param name="newController">The new controller.</param>
        protected virtual void OnControllerChanged(IGraphController<UwpGraphDataSeries> oldController, IGraphController<UwpGraphDataSeries> newController)
        {
            if (oldController != null)
            {
                oldController.Surface = null;
            }

            if (newController != null)
            {
                newController.Surface = this;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Graph Surface Size Changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void GraphSurface_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _size = new System.Drawing.SizeF((float)e.NewSize.Width, (float)e.NewSize.Height);
            _size_changed = true;
        }

        #endregion

        #region IGraphSurface

        /// <summary>
        /// Called before drawing has started.
        /// </summary>
        public void BeginDraw()
        {
            if (_size_changed)
            {
                CanvasDevice device = CanvasDevice.GetSharedDevice();
                _bitmap = new CanvasRenderTarget(device, _size.Width, _size.Height, 96);
                _size_changed = false;
            }

            _session = _bitmap.CreateDrawingSession();
            _session.Clear(Colors.Transparent);

        }

        /// <summary>
        /// Applies transformation on the current pass.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public void SetTransform(GraphTransform transform)
        {
            _session.Transform = Matrix3x2.CreateTranslation(transform.TranslateX / transform.ScaleX, transform.TranslateY / transform.ScaleY) * Matrix3x2.CreateScale(transform.ScaleX, transform.ScaleY);
        }

        /// <summary>
        /// Draws the stroke of the specified data series points.
        /// </summary>
        /// <param name="dataSeries">The data series.</param>
        /// <param name="points">The points.</param>
        public void DrawSeries(UwpGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points)
        {
            List<Vector2> vPoints = points.Select(x => new Vector2(x.X, x.Y)).ToList();
            _session.DrawPolyline(vPoints, dataSeries.Stroke, dataSeries.StrokeThickness);
        }

        /// <summary>
        /// Fills the specified data series points.
        /// </summary>
        /// <param name="dataSeries">The data series.</param>
        /// <param name="points">The points.</param>
        public void FillSeries(UwpGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points)
        {
            List<Vector2> vPoints = points.Select(x => new Vector2(x.X, x.Y)).ToList();
            _session.FillPolygon(this, dataSeries, vPoints, dataSeries.Fill);
        }

        /// <summary>
        /// Called when drawing has completed.
        /// </summary>
        public void EndDraw()
        {
            _canvas2d.Invalidate();
            _session.Dispose();
        }

        /// <summary>
        /// Returns the actual size of the surface.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.SizeF GetSize()
        {
            return _size;
        }

        /// <summary>
        /// Returns the current bounds of the zooming rectangle.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.RectangleF GetZoomRect()
        {
            return _zoom_rect;
        }

        #endregion
    }
}
