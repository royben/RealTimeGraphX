using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents a panel that will align its children in an axis labels like arrangement.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Grid" />
    public class UwpGraphAxisPanel : Grid
    {
        private List<object> _measured_childs;

        /// <summary>
        /// Gets or sets the panel orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(UwpGraphAxisPanel), new PropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Initializes a new instance of the <see cref="UwpGraphAxisPanel"/> class.
        /// </summary>
        public UwpGraphAxisPanel()
        {
            _measured_childs = new List<object>();
            Loaded += VerticalAxisPanel_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event of the VerticalAxisGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void VerticalAxisPanel_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePanel();
        }

        /// <summary>
        /// Provides the behavior for the "Measure" pass of the layout cycle. Classes can override this method to define their own "Measure" pass behavior.
        /// </summary>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects or based on other considerations such as a fixed container size.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            UpdatePanel();
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Updates the panel.
        /// </summary>
        private void UpdatePanel()
        {
            if (!Children.Any(x => !_measured_childs.Contains(x))) return;

            RowDefinitions.Clear();
            ColumnDefinitions.Clear();


            if (Orientation == Orientation.Vertical)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    FrameworkElement element = Children[i] as FrameworkElement;

                    _measured_childs.Add(element);

                    if (i < Children.Count - 1)
                    {
                        RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                        Grid.SetRow(element, i);

                        //if (element.DataContext is GraphAxisTickData)
                        //{
                        //    (element.DataContext as GraphAxisTickData).Row = i;
                        //}

                        element.VerticalAlignment = VerticalAlignment.Top;

                        element.SizeChanged += (_, __) =>
                        {
                            element.Margin = new Thickness(0, (element.ActualHeight / 2) * -1, 0, 0);
                        };
                    }
                    else
                    {
                        Grid.SetRow(element, i);

                        //if (element.DataContext is GraphAxisTickData)
                        //{
                        //    (element.DataContext as GraphAxisTickData).Row = i;
                        //}

                        element.VerticalAlignment = VerticalAlignment.Bottom;

                        element.SizeChanged += (_, __) =>
                        {
                            element.Margin = new Thickness(0, 0, 0, (element.ActualHeight / 2) * -1);
                        };
                    }
                }
            }
            else
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    FrameworkElement element = Children[i] as FrameworkElement;

                    _measured_childs.Add(element);

                    if (i < Children.Count - 1)
                    {
                        ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        Grid.SetColumn(element, i);
                        element.HorizontalAlignment = HorizontalAlignment.Left;

                        //if (element.DataContext is GraphAxisTickData)
                        //{
                        //    (element.DataContext as GraphAxisTickData).Column = i;
                        //}

                        element.SizeChanged += (_, __) =>
                        {
                            element.Margin = new Thickness((element.ActualWidth / 2) * -1, 0, 0, 0);
                        };
                    }
                    else
                    {
                        Grid.SetColumn(element, i);
                        element.HorizontalAlignment = HorizontalAlignment.Right;

                        //if (element.DataContext is GraphAxisTickData)
                        //{
                        //    (element.DataContext as GraphAxisTickData).Column = i;
                        //}

                        element.SizeChanged += (_, __) =>
                        {
                            element.Margin = new Thickness(0, 0, (element.ActualWidth / 2) * -1, 0);
                        };
                    }
                }
            }
        }
    }
}
