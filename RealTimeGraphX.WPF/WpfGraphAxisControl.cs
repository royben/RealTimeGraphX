﻿using RealTimeGraphX.EventArguments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a horizontal/vertical graph axis component.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.WPF.WpfGraphComponentBase" />
    public class WpfGraphAxisControl : WpfGraphComponentBase
    {
        private ItemsControl _items_control;
        private WpfGraphAxisPanel _axisPanel;

        /// <summary>
        /// Initializes the <see cref="WpfGraphAxisControl"/> class.
        /// </summary>
        static WpfGraphAxisControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfGraphAxisControl), new FrameworkPropertyMetadata(typeof(WpfGraphAxisControl)));
        }

        /// <summary>
        /// Gets or sets the control orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WpfGraphAxisControl), new PropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Gets or sets the tick item template.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(WpfGraphAxisControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the tick items.
        /// </summary>
        internal ObservableCollection<WpfGraphAxisTickData> Items
        {
            get { return (ObservableCollection<WpfGraphAxisTickData>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<WpfGraphAxisTickData>), typeof(WpfGraphAxisControl), new PropertyMetadata(null));

        internal int Ticks => Divisions + 1 ;

        /// <summary>
        /// Gets or sets the number of divisions to display on the control.
        /// </summary>
        public int Divisions
        {
            get { return (int)GetValue(DivisionsProperty); }
            set { SetValue(DivisionsProperty, value); }
        }
        public static readonly DependencyProperty DivisionsProperty =
            DependencyProperty.Register("Divisions", typeof(int), typeof(WpfGraphAxisControl), new PropertyMetadata(8, (d, e) => (d as WpfGraphAxisControl).OnDivisionsChanged()));

        /// <summary>
        /// Gets or sets the string format which is used to format the ticks value.
        /// </summary>
        public String StringFormat
        {
            get { return (String)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(String), typeof(WpfGraphAxisControl), new PropertyMetadata(null));

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _items_control = GetTemplateChild("PART_ItemsControl") as ItemsControl;

            _items_control.Loaded += (x, e) =>
            {
                ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(_items_control);
                _axisPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as WpfGraphAxisPanel;
            };

            OnDivisionsChanged();
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

#if false
        /// <summary>
        /// Called when the <see cref="Ticks"/> property has changed.
        /// </summary>
        protected virtual void OnTicksChanged()
        {
            Items = new ObservableCollection<WpfGraphAxisTickData>(Enumerable.Range(0, Ticks).Select(x => new WpfGraphAxisTickData()));

            Controller?.RequestVirtualRangeChange();

            _axisPanel?.UpdatePanel();
        }
#endif

        /// <summary>
        /// Called when the <see cref="Divisions"/> property has changed.
        /// </summary>
        protected virtual void OnDivisionsChanged()
        {
            Items = new ObservableCollection<WpfGraphAxisTickData>(Enumerable.Range(0, Ticks).Select(x => new WpfGraphAxisTickData()));

            Controller?.RequestVirtualRangeChange();

            _axisPanel?.UpdatePanel();
        }

        protected override void OnControllerChanged(IGraphController oldController, IGraphController newController)
        {
            base.OnControllerChanged(oldController, newController);

            if (newController != null)
            {
                newController.RequestVirtualRangeChange();
            }
        }

        /// <summary>
        /// Handles the <see cref="IGraphController.VirtualRangeChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected override void OnVirtualRangeChanged(object sender, RangeChangedEventArgs e)
        {
            InvokeUI(() =>
            {
                if (Orientation == Orientation.Vertical)
                {
                    if (Items != null)
                    {
                        var steps = e.MinimumY.CreateRange(e.MinimumY, e.MaximumY, Ticks).Reverse().ToList();

                        for (int i = 0; i < steps.Count; i++)
                        {
                            var tick_data = Items[i];
                            tick_data.Data = steps[i];
                            tick_data.DisplayText = tick_data.Data.ToString(StringFormat);
                            tick_data.IsFirst = i == 0;
                            tick_data.IsLast = i == steps.Count - 1;
                            tick_data.IsEven = i % 2 == 0;
                        }
                    }
                }
                else
                {
                    if (Items != null)
                    {
                        var steps = e.MinimumX.CreateRange(e.MinimumX, e.MaximumX, Ticks).ToList();

                        for (int i = 0; i < steps.Count; i++)
                        {
                            var tick_data = Items[i];
                            tick_data.Data = steps[i];
                            tick_data.DisplayText = tick_data.Data.ToString(StringFormat);
                            tick_data.IsFirst = i == 0;
                            tick_data.IsLast = i == steps.Count - 1;
                            tick_data.IsEven = i % 2 == 0;
                        }
                    }
                }
            });
        }
    }
}
