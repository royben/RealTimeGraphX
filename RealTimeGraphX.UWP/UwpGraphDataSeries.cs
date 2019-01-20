using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
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
    /// Represents a UWP <see cref="IGraphDataSeries">data series</see>.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphObject" />
    /// <seealso cref="RealTimeGraphX.IGraphDataSeries" />
    public class UwpGraphDataSeries : GraphObject, IGraphDataSeries
    {
        private class Stop
        {
            public float Offset { get; set; }
            public Color Color { get; set; }
        }
        private Stop[] _stopCollection;



        private String _name;
        /// <summary>
        /// Gets or sets the series name.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChangedAuto(); }
        }

        private float _strokeThickness;
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public float StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                RaisePropertyChangedAuto();
            }
        }

        private bool _isVisible;
        /// <summary>
        /// Gets or sets a value indicating whether this series should be visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; RaisePropertyChangedAuto(); }
        }

        private Color _stroke;
        /// <summary>
        /// Gets or sets the series stroke color.
        /// </summary>
        public Color Stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                RaisePropertyChangedAuto();
            }
        }

        private Brush _fill;
        /// <summary>
        /// Gets or sets the series fill brush.
        /// </summary>
        public Brush Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;

                if (_fill != null)
                {
                    if (_fill is SolidColorBrush)
                    {
                        _stopCollection = new Stop[] { new Stop() { Color = (_fill as SolidColorBrush).Color, Offset = 0 } };
                    }
                    else if (_fill is LinearGradientBrush)
                    {
                        _stopCollection = (_fill as LinearGradientBrush).GradientStops.Select(x => new Stop() { Color = x.Color, Offset = (float)x.Offset }).ToArray();
                    }
                }

                RaisePropertyChangedAuto();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to fill the series.
        /// </summary>
        public bool UseFill
        {
            get { return Fill != null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfDataSeries"/> class.
        /// </summary>
        public UwpGraphDataSeries()
        {
            StrokeThickness = 1;
            IsVisible = true;
            Stroke = Colors.DodgerBlue;
        }

        /// <summary>
        /// Gets the canvas brush.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        internal ICanvasBrush GetCanvasBrush(ICanvasResourceCreator creator, double width, double height)
        {
            if (_stopCollection.Length == 1)
            {
                return new CanvasSolidColorBrush(creator, _stopCollection[0].Color);
            }
            else
            {
                return new CanvasLinearGradientBrush(creator, _stopCollection.Select(x => new CanvasGradientStop() { Color = x.Color, Position = (float)(x.Offset) }).ToArray()) { StartPoint = new System.Numerics.Vector2(0, 0), EndPoint = new System.Numerics.Vector2((float)width, (float)height) };
            }
        }
    }
}
