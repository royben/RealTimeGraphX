using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents an invisible control which can be used to bind data to an element inside a style which is not possible using UWP styles.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    internal class UwpBindingElement : Control
    {
        private long _callback_token;
        private DependencyProperty _element_dp;
        private DependencyProperty _datacontext_dp;
        private FrameworkElement _parent;
        private FrameworkElement _element;

        #region Properties

        /// <summary>
        /// Gets or sets the name of the element to create the binding for.
        /// </summary>
        public String ElementName
        {
            get { return (String)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }
        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(String), typeof(UwpBindingElement), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the element property path for the binding target.
        /// </summary>
        public String ElementPath
        {
            get { return (String)GetValue(ElementPathProperty); }
            set { SetValue(ElementPathProperty, value); }
        }
        public static readonly DependencyProperty ElementPathProperty =
            DependencyProperty.Register("ElementPath", typeof(String), typeof(UwpBindingElement), new PropertyMetadata(null, (d, e) => (d as UwpBindingElement).OnBindingChanged()));

        /// <summary>
        /// Gets or sets the data context property path as the source for the binding.
        /// </summary>
        public String DataContextPath
        {
            get { return (String)GetValue(DataContextPathProperty); }
            set { SetValue(DataContextPathProperty, value); }
        }
        public static readonly DependencyProperty DataContextPathProperty =
            DependencyProperty.Register("DataContextPath", typeof(String), typeof(UwpBindingElement), new PropertyMetadata(null, (d, e) => (d as UwpBindingElement).OnBindingChanged()));

        /// <summary>
        /// Gets or sets the optional converter parameter.
        /// </summary>
        public Object ConverterParameter
        {
            get { return (Object)GetValue(ConverterParameterProperty); }
            set { SetValue(ConverterParameterProperty, value); }
        }
        public static readonly DependencyProperty ConverterParameterProperty =
            DependencyProperty.Register("ConverterParameter", typeof(Object), typeof(UwpBindingElement), new PropertyMetadata(null, (d, e) => (d as UwpBindingElement).OnBindingChanged()));

        /// <summary>
        /// Gets or sets the optional binding converter.
        /// </summary>
        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }
        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register("Converter", typeof(IValueConverter), typeof(UwpBindingElement), new PropertyMetadata(null, (d, e) => (d as UwpBindingElement).OnBindingChanged()));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UwpBindingElement"/> class.
        /// </summary>
        public UwpBindingElement()
        {
            Loaded += BindingElement_Loaded;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Loaded event of the BindingElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BindingElement_Loaded(object sender, RoutedEventArgs e)
        {
            _parent = VisualTreeHelper.GetParent(this) as FrameworkElement;
            _element = (_parent as FrameworkElement).FindName(ElementName) as FrameworkElement;
            OnBindingChanged();
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Called when the binded element data context has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DataContextChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnElementDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            UpdateBinding();
        }

        /// <summary>
        /// Called when the <see cref="DataContextPath"/> property has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dp">The dependency property.</param>
        protected virtual void OnDataContextPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            var value = ((_element as FrameworkElement).DataContext as DependencyObject).GetValue(_datacontext_dp);

            if (Converter != null)
            {
                value = Converter.Convert(value, null, ConverterParameter, null);
            }

            _element.SetValue(_element_dp, value);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Reevalutes the bindind.
        /// </summary>
        private void OnBindingChanged()
        {
            if (_element != null)
            {
                _element.DataContextChanged -= OnElementDataContextChanged;
                _element.DataContextChanged += OnElementDataContextChanged;

                UpdateBinding();
            }
        }

        /// <summary>
        /// Updates the binding.
        /// </summary>
        private void UpdateBinding()
        {
            if (_element != null && _element.DataContext != null && ElementPath != null && DataContextPath != null)
            {
                var dataContext = _element.DataContext as DependencyObject;

                if (dataContext != null)
                {
                    var elementPropInfo = _element.GetType().GetProperty(ElementPath + "Property", BindingFlags.Static | BindingFlags.Public);
                    var dataContextPropInfo = DataContext.GetType().GetField(DataContextPath + "Property", BindingFlags.Static | BindingFlags.Public);

                    if (elementPropInfo != null && dataContextPropInfo != null)
                    {
                        _element_dp = elementPropInfo.GetValue(_element) as DependencyProperty;
                        _datacontext_dp = dataContextPropInfo.GetValue(dataContext) as DependencyProperty;

                        if (_element_dp != null && _datacontext_dp != null)
                        {
                            if (_callback_token != 0)
                            {
                                dataContext.UnregisterPropertyChangedCallback(_datacontext_dp, _callback_token);
                            }

                            _callback_token = dataContext.RegisterPropertyChangedCallback(_datacontext_dp, OnDataContextPropertyChanged);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
