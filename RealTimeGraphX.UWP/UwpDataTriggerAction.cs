using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents a data trigger action.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.DependencyObject" />
    /// <seealso cref="Microsoft.Xaml.Interactivity.IAction" />
    internal class UwpDataTriggerAction : DependencyObject, IAction
    {
        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        public FrameworkElement TargetObject
        {
            get { return (FrameworkElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(UwpDataTriggerAction), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public String PropertyName
        {
            get { return (String)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(String), typeof(UwpDataTriggerAction), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public Object Value
        {
            get { return (Object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Object), typeof(UwpDataTriggerAction), new PropertyMetadata(null));

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="T:System.Object" /> that is passed to the action by the behavior. Generally this is <seealso cref="P:Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject" /> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>
        /// Returns the result of the action.
        /// </returns>
        /// <remarks>
        /// An example of parameter usage is EventTriggerBehavior, which passes the EventArgs as a parameter to its actions.
        /// </remarks>
        public object Execute(object sender, object parameter)
        {
            List<PropertyInfo> props = new List<PropertyInfo>();
            List<FieldInfo> fields = new List<FieldInfo>();

            Type type = TargetObject.GetType();

            while (type != null)
            {
                props.AddRange(type.GetTypeInfo().DeclaredProperties);
                fields.AddRange(type.GetTypeInfo().DeclaredFields);
                type = type.BaseType;

                var prop = props.SingleOrDefault(x => x.Name == PropertyName + "Property");
                
                if (prop != null)
                {
                    var propValue = props.SingleOrDefault(x => x.Name == PropertyName);
                    var _element_dp = prop.GetValue(TargetObject) as DependencyProperty;
                    TargetObject.SetValue(_element_dp, Convert.ChangeType(Value, propValue.PropertyType));
                    return null;
                }
                else
                {
                    var field = fields.SingleOrDefault(x => x.Name == PropertyName + "Property");

                    if (field != null)
                    {
                        var propValue = props.SingleOrDefault(x => x.Name == PropertyName);

                        var _element_dp = field.GetValue(TargetObject) as DependencyProperty;
                        TargetObject.SetValue(_element_dp, Convert.ChangeType(Value, propValue.PropertyType));
                        return null;
                    }
                }
            }

            return null;
        }
    }
}
