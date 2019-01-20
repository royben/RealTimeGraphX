using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a WPF <see cref="IGraphDataSeries">data series</see>.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphObject" />
    /// <seealso cref="RealTimeGraphX.IGraphDataSeries" />
    public class WpfGraphDataSeries : GraphObject, IGraphDataSeries
    {
        #region Internal Properties

        /// <summary>
        /// Gets the GDI stroke color.
        /// </summary>
        internal System.Drawing.Color GdiStroke { get; private set; }

        /// <summary>
        /// Gets the GDI fill brush.
        /// </summary>
        internal System.Drawing.Brush GdiFill { get; private set; }

        /// <summary>
        /// Gets or sets the GDI pen.
        /// </summary>
        internal System.Drawing.Pen GdiPen { get; set; }

        #endregion

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
                GdiPen = new System.Drawing.Pen(GdiStroke, _strokeThickness);
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

                if (_stroke != null)
                {
                    GdiStroke = _stroke.ToGdiColor();
                    GdiPen = new System.Drawing.Pen(GdiStroke, StrokeThickness);
                }
                else
                {
                    GdiStroke = System.Drawing.Color.Transparent;
                }
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
                RaisePropertyChangedAuto();

                if (_fill != null)
                {
                    GdiFill = _fill.ToGdiBrush();
                }
                else
                {
                    GdiFill = null;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfGraphDataSeries"/> class.
        /// </summary>
        public WpfGraphDataSeries()
        {
            StrokeThickness = 1;
            IsVisible = true;
            Stroke = Colors.DodgerBlue;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to fill the series.
        /// </summary>
        public bool UseFill
        {
            get { return Fill != null; }
        }
    }
}
