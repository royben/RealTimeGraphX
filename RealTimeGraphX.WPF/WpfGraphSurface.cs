using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RealTimeGraphX.EventArguments;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a WPF <see cref="IGraphSurface{TDataSeries}">graph surface</see>.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    /// <seealso cref="RealTimeGraphX.IGraphSurface{RealTimeGraphX.WPF.WpfGraphDataSeries}" />
    public class WpfGraphSurface : Control, IGraphSurface<WpfGraphDataSeries>
    {
        private WriteableBitmap _writeable_bitmap;
        private System.Drawing.Bitmap _gdi_bitmap;
        private System.Drawing.Graphics _g;

        private bool _size_changed;
        private System.Drawing.SizeF _size;
        private System.Drawing.RectangleF _zoom_rect;
        private Rectangle _selection_rectangle;
        private Canvas _selection_canvas;
        private bool _is_selection_mouse_down_zoom;
        private bool _is_selection_mouse_down_pan;
        private Point _selection_start_point;
        private bool _is_scaled;
        private Point _current_mouse_position;
        private Point _last_mouse_position;
        private Grid _grid;

        #region Properties

        /// <summary>
        /// Gets or sets current graph rendered image.
        /// </summary>
        public BitmapSource Image
        {
            get { return (BitmapSource)GetValue(ImageProperty); }
            private set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapSource), typeof(WpfGraphSurface), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the graph controller.
        /// </summary>
        public IGraphController<WpfGraphDataSeries> Controller
        {
            get { return (IGraphController<WpfGraphDataSeries>)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IGraphController<WpfGraphDataSeries>), typeof(WpfGraphSurface), new PropertyMetadata(null, (d, e) => (d as WpfGraphSurface).OnControllerChanged(e.OldValue as IGraphController<WpfGraphDataSeries>, e.NewValue as IGraphController<WpfGraphDataSeries>)));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="WpfGraphSurface"/> class.
        /// </summary>
        static WpfGraphSurface()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfGraphSurface), new FrameworkPropertyMetadata(typeof(WpfGraphSurface)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfGraphSurface"/> class.
        /// </summary>
        public WpfGraphSurface()
        {
            SizeChanged += WpfGraphSurface_SizeChanged;
        }

        #endregion

        #region Apply Template

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _selection_rectangle = GetTemplateChild("PART_SelectionRectangle") as Rectangle;
            _selection_canvas = GetTemplateChild("PART_SelectionCanvas") as Canvas;
            _grid = GetTemplateChild("PART_Grid") as Grid;

            _selection_canvas.MouseDown += OnSelectionCanvasMouseDown;
            _selection_canvas.MouseUp += OnSelectionCanvasMouseUp;
            _selection_canvas.MouseMove += OnSelectionCanvasMouseMove;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when the mouse moves over the zoom/pan selection canvas.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectionCanvasMouseMove(object sender, MouseEventArgs e)
        {
            _current_mouse_position = e.GetPosition(_selection_canvas);

            if (_is_selection_mouse_down_zoom && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Canvas.SetLeft(_selection_rectangle, _selection_start_point.X);
                Canvas.SetTop(_selection_rectangle, _selection_start_point.Y);

                Point _selection_current_point = e.GetPosition(_selection_canvas);

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
                Point _selection_current_point = e.GetPosition(_selection_canvas);

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
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectionCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            _selection_canvas.ReleaseMouseCapture();

            if (_is_selection_mouse_down_pan)
            {
                _is_selection_mouse_down_pan = false;
            }
            else if (_is_selection_mouse_down_zoom)
            {
                _is_selection_mouse_down_zoom = false;

                _zoom_rect = new System.Drawing.RectangleF((float)Canvas.GetLeft(_selection_rectangle), (float)Canvas.GetTop(_selection_rectangle), (float)_selection_rectangle.Width, (float)_selection_rectangle.Height);
                _selection_rectangle.Visibility = Visibility.Hidden;
                _is_scaled = true;
            }
        }

        /// <summary>
        /// Called when the mouse pressed on the zoom/pan selection canvas.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectionCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(_selection_canvas);

            _selection_start_point = e.GetPosition(_selection_canvas);
            _current_mouse_position = _selection_start_point;
            _last_mouse_position = _current_mouse_position;

            if (e.ClickCount == 2)
            {
                _zoom_rect = new System.Drawing.RectangleF();
                _is_scaled = false;
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
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
        /// Called when the <see cref="Controller"/> property has changed.
        /// </summary>
        /// <param name="oldController">The old controller.</param>
        /// <param name="newController">The new controller.</param>
        protected virtual void OnControllerChanged(IGraphController<WpfGraphDataSeries> oldController, IGraphController<WpfGraphDataSeries> newController)
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

        #region IGraphSurface

        /// <summary>
        /// Called before drawing has started.
        /// </summary>
        public void BeginDraw()
        {
            if (_size_changed)
            {
                _writeable_bitmap = new WriteableBitmap((int)Math.Max(_size.Width, 1), (int)Math.Max(_size.Height, 1), 96.0, 96.0, PixelFormats.Pbgra32, null);

                _gdi_bitmap = new System.Drawing.Bitmap(_writeable_bitmap.PixelWidth, _writeable_bitmap.PixelHeight,
                                             _writeable_bitmap.BackBufferStride,
                                             System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                             _writeable_bitmap.BackBuffer);

                _size_changed = false;
            }

            _writeable_bitmap.Lock();

            _g = System.Drawing.Graphics.FromImage(_gdi_bitmap);
            _g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            _g.Clear(System.Drawing.Color.Transparent);
        }

        /// <summary>
        /// Applies transformation on the current pass.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public void SetTransform(GraphTransform transform)
        {
            _g.TranslateTransform((float)transform.TranslateX, (float)transform.TranslateY);
            _g.ScaleTransform((float)transform.ScaleX, (float)transform.ScaleY);
        }

        /// <summary>
        /// Draws the stroke of the specified data series points.
        /// </summary>
        /// <param name="dataSeries">The data series.</param>
        /// <param name="points">The points.</param>
        public void DrawSeries(WpfGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points)
        {
            _g.DrawCurve(dataSeries.GdiPen, points.ToArray());
        }

        /// <summary>
        /// Fills the specified data series points.
        /// </summary>
        /// <param name="dataSeries">The data series.</param>
        /// <param name="points">The points.</param>
        public void FillSeries(WpfGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points)
        {
            var brush = dataSeries.GdiFill;

            if (dataSeries.GdiFill is System.Drawing.Drawing2D.LinearGradientBrush)
            {
                var gradient = dataSeries.GdiFill as System.Drawing.Drawing2D.LinearGradientBrush;
                gradient.ResetTransform();
                gradient.ScaleTransform((_size.Width / gradient.Rectangle.Width), (_size.Height / gradient.Rectangle.Height));
            }

            _g.FillPolygon(dataSeries.GdiFill, points.ToArray());
        }

        /// <summary>
        /// Called when drawing has completed.
        /// </summary>
        public void EndDraw()
        {
            _writeable_bitmap.AddDirtyRect(new Int32Rect(0, 0, _writeable_bitmap.PixelWidth, _writeable_bitmap.PixelHeight));
            _writeable_bitmap.Unlock();

            var cloned = _writeable_bitmap.Clone();
            cloned.Freeze();

            Dispatcher.BeginInvoke(new Action((() =>
            {
                Image = cloned;
            })));

            _g.Dispose();
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

        #region Event Handlers

        /// <summary>
        /// Handles the WPF Graph Surface Size Changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WpfGraphSurface_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _size = new System.Drawing.SizeF((float)e.NewSize.Width, (float)e.NewSize.Height);
            _size_changed = true;
        }

        #endregion
    }
}
