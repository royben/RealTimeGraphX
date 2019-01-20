using Microsoft.Xaml.Interactions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents an invisible element which can be used to reevaluate a data trigger behavior binding inside a style.  
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    internal class UwpDataTriggerElement : Control
    {
        private FrameworkElement _parent;
        private DataTriggerBehavior _dataTrigger;

        #region Properties

        /// <summary>
        /// Gets or sets the name of the data trigger behavior element.
        /// </summary>
        public String ElementName
        {
            get { return (String)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }
        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(String), typeof(UwpDataTriggerElement), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the data context path that will cause the evaluation when changed.
        /// </summary>
        public String DataContextPath
        {
            get { return (String)GetValue(DataContextPathProperty); }
            set { SetValue(DataContextPathProperty, value); }
        }
        public static readonly DependencyProperty DataContextPathProperty =
            DependencyProperty.Register("DataContextPath", typeof(String), typeof(UwpDataTriggerElement), new PropertyMetadata(null, (d, e) => (d as UwpDataTriggerElement).OnBindingChanged()));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UwpDataTriggerElement"/> class.
        /// </summary>
        public UwpDataTriggerElement()
        {
            Loaded += DataTriggerElement_Loaded;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Loaded event of the DataTriggerElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DataTriggerElement_Loaded(object sender, RoutedEventArgs e)
        {
            _parent = VisualTreeHelper.GetParent(this) as FrameworkElement;
            _dataTrigger = (_parent as FrameworkElement).FindName(ElementName) as DataTriggerBehavior;
            OnBindingChanged();
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Called when the <see cref="DataContextPathProperty"/> has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dp">The dependency property.</param>
        protected virtual void OnDataContextPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            var value = (DataContext as DependencyObject).GetValue(dp);

            if (value.ToString() == _dataTrigger.Value.ToString())
            {
                UpdateBinding();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reevaluates the binding.
        /// </summary>
        private void OnBindingChanged()
        {
            if (DataContext != null)
            {
                var dataContextPropInfo = DataContext.GetType().GetField(DataContextPath + "Property", BindingFlags.Static | BindingFlags.Public);

                if (dataContextPropInfo != null)
                {
                    var _datacontext_dp = dataContextPropInfo.GetValue(DataContext) as DependencyProperty;

                    (DataContext as DependencyObject).RegisterPropertyChangedCallback(_datacontext_dp, OnDataContextPropertyChanged);

                    if ((DataContext as DependencyObject).GetValue(_datacontext_dp).ToString() == _dataTrigger.Value.ToString())
                    {
                        UpdateBinding();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the binding.
        /// </summary>
        private void UpdateBinding()
        {
            foreach (var action in _dataTrigger.Actions.OfType<UwpDataTriggerAction>())
            {
                action.Execute(_dataTrigger, _dataTrigger.Value);
            }
        }

        #endregion
    }
}
