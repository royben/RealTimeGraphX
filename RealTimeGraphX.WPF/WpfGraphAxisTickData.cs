using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a graph axis data point tick value wrapper.
    /// </summary>
    public class WpfGraphAxisTickData : DependencyObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether this tick is the first tick.
        /// </summary>
        public bool IsFirst
        {
            get { return (bool)GetValue(IsFirstProperty); }
            set { SetValue(IsFirstProperty, value); }
        }
        public static readonly DependencyProperty IsFirstProperty =
            DependencyProperty.Register("IsFirst", typeof(bool), typeof(WpfGraphAxisTickData), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether this tick is the last tick.
        /// </summary>
        public bool IsLast
        {
            get { return (bool)GetValue(IsLastProperty); }
            set { SetValue(IsLastProperty, value); }
        }
        public static readonly DependencyProperty IsLastProperty =
            DependencyProperty.Register("IsLast", typeof(bool), typeof(WpfGraphAxisTickData), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether this tick is not the first or last.
        /// </summary>
        public bool IsCenter
        {
            get { return !IsFirst && !IsLast; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this tick index is even.
        /// </summary>
        public bool IsEven
        {
            get { return (bool)GetValue(IsEvenProperty); }
            set { SetValue(IsEvenProperty, value); }
        }
        public static readonly DependencyProperty IsEvenProperty =
            DependencyProperty.Register("IsEven", typeof(bool), typeof(WpfGraphAxisTickData), new PropertyMetadata(false));

        /// <summary>
        /// Gets a value indicating whether this tick index is odd.
        /// </summary>
        public bool IsOdd
        {
            get { return !IsEven; }
        }

        /// <summary>
        /// Gets or sets the actual graph data point.
        /// </summary>
        public IGraphDataPoint Data
        {
            get { return (IGraphDataPoint)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(IGraphDataPoint), typeof(WpfGraphAxisTickData), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        public String DisplayText
        {
            get { return (String)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }
        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText", typeof(String), typeof(WpfGraphAxisTickData), new PropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="Data">Data Point</see> value.
        /// </summary>
        public object Value
        {
            get
            {
                return Data != null ? Data.GetValue() : null;
            }
        }
    }
}
