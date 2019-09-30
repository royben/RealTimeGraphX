using RealTimeGraphX.EventArguments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents a horizontal/vertical graph axis control.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphSurfaceComponentBase" />
    public class UwpGraphAxisControl : UwpGraphComponentBase
    {
        private ItemsControl _items_control;

        /// <summary>
        /// Gets or sets the control orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(UwpGraphAxisControl), new PropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Gets or sets the tick item template.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(UwpGraphAxisControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the tick items.
        /// </summary>
        internal ObservableCollection<UwpGraphAxisTickData> Items
        {
            get { return (ObservableCollection<UwpGraphAxisTickData>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<UwpGraphAxisTickData>), typeof(UwpGraphAxisControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the number of ticks to display on the control.
        /// </summary>
        public int Ticks
        {
            get { return (int)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        public static readonly DependencyProperty TicksProperty =
            DependencyProperty.Register("Ticks", typeof(int), typeof(UwpGraphAxisControl), new PropertyMetadata(9, (d, e) => (d as UwpGraphAxisControl).OnTicksChanged()));

        /// <summary>
        /// Gets or sets the string format which is used to format the ticks value.
        /// </summary>
        public String StringFormat
        {
            get { return (String)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(String), typeof(UwpGraphAxisControl), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="UwpGraphAxisControl"/> class.
        /// </summary>
        public UwpGraphAxisControl()
        {
            this.DefaultStyleKey = typeof(UwpGraphAxisControl);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _items_control = GetTemplateChild("PART_ItemsControl") as ItemsControl;
            OnTicksChanged();
        }

        /// <summary>
        /// Called when the <see cref="Ticks"/> property has changed.
        /// </summary>
        protected virtual void OnTicksChanged()
        {
            Items = new ObservableCollection<UwpGraphAxisTickData>(Enumerable.Range(0, Ticks).Select(x => new UwpGraphAxisTickData()));

            if (Controller != null)
            {
                Controller.RequestVirtualRangeChange();
            }
        }

        /// <summary>
        /// Called when the controller has changed.
        /// </summary>
        /// <param name="oldController">The old controller.</param>
        /// <param name="newController">The new controller.</param>
        protected override void OnControllerChanged(IGraphController oldController, IGraphController newController)
        {
            base.OnControllerChanged(oldController, newController);

            if (newController != null)
            {
                newController.RequestVirtualRangeChange();
            }
        }

        /// <summary>
        /// Handles the <see cref="IGraphController.VirtualRangeChanged" /> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected override void OnVirtualRangeChanged(object sender, RangeChangedEventArgs e)
        {
            base.OnVirtualRangeChanged(sender, e);

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
