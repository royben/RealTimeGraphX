using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a graph grid lines component.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphSurfaceComponentBase" />
    public class WpfGraphGridLines : WpfGraphComponentBase
    {
        /// <summary>
        /// Gets or sets the number of grid rows.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(WpfGraphGridLines), new PropertyMetadata(8, (d, e) => (d as WpfGraphGridLines).UpdateGridLines()));

        /// <summary>
        /// Gets or sets the number of grid columns.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(WpfGraphGridLines), new PropertyMetadata(8, (d, e) => (d as WpfGraphGridLines).UpdateGridLines()));

        /// <summary>
        /// Gets or sets the vertical items.
        /// </summary>
        internal IEnumerable<int> VerticalItems
        {
            get { return (IEnumerable<int>)GetValue(VerticalItemsProperty); }
            set { SetValue(VerticalItemsProperty, value); }
        }
        internal static readonly DependencyProperty VerticalItemsProperty =
            DependencyProperty.Register("VerticalItems", typeof(IEnumerable<int>), typeof(WpfGraphGridLines), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the horizontal items.
        /// </summary>
        internal IEnumerable<int> HorizontalItems
        {
            get { return (IEnumerable<int>)GetValue(HorizontalItemsProperty); }
            set { SetValue(HorizontalItemsProperty, value); }
        }
        internal static readonly DependencyProperty HorizontalItemsProperty =
            DependencyProperty.Register("HorizontalItems", typeof(IEnumerable<int>), typeof(WpfGraphGridLines), new PropertyMetadata(null));

        /// <summary>
        /// Initializes the <see cref="WpfGraphGridLines"/> class.
        /// </summary>
        static WpfGraphGridLines()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfGraphGridLines), new FrameworkPropertyMetadata(typeof(WpfGraphGridLines)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfGraphGridLines"/> class.
        /// </summary>
        public WpfGraphGridLines()
        {
            Loaded += GridLines_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event of the GridLines control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GridLines_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGridLines();
        }

        /// <summary>
        /// Updates the grid lines.
        /// </summary>
        private void UpdateGridLines()
        {
            VerticalItems = Enumerable.Range(0, Rows).ToList();
            HorizontalItems = Enumerable.Range(0, Columns).ToList();
        }
    }
}
