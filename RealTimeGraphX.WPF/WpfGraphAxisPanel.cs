using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a panel that will align its children in an axis labels like arrangement.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Grid" />
    public class WpfGraphAxisPanel : Grid
    {
        /// <summary>
        /// Gets or sets the panel orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WpfGraphAxisPanel), new PropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfGraphAxisPanel"/> class.
        /// </summary>
        public WpfGraphAxisPanel()
        {
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
        /// Updates the panel.
        /// </summary>
        private void UpdatePanel()
        {
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();


            if (Orientation == Orientation.Vertical)
            {
                for (int i = 0; i < InternalChildren.Count; i++)
                {
                    FrameworkElement element = InternalChildren[i] as FrameworkElement;

                    if (i < InternalChildren.Count - 1)
                    {
                        RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                        Grid.SetRow(element, i);
                        element.VerticalAlignment = VerticalAlignment.Top;

                        element.SizeChanged += (_, __) =>
                        {
                            element.Margin = new Thickness(0, (element.ActualHeight / 2) * -1, 0, 0);
                        };
                    }
                    else
                    {
                        Grid.SetRow(element, i);
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
                for (int i = 0; i < InternalChildren.Count; i++)
                {
                    FrameworkElement element = InternalChildren[i] as FrameworkElement;

                    if (i < InternalChildren.Count - 1)
                    {
                        ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        Grid.SetColumn(element, i);
                        element.HorizontalAlignment = HorizontalAlignment.Left;

                        element.SizeChanged += (_, __) =>
                        {
                            element.Margin = new Thickness((element.ActualWidth / 2) * -1, 0, 0, 0);
                        };
                    }
                    else
                    {
                        Grid.SetColumn(element, i);
                        element.HorizontalAlignment = HorizontalAlignment.Right;

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
