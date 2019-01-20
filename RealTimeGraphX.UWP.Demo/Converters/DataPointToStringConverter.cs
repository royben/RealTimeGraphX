using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace RealTimeGraphX.UWP.Demo.Converters
{
    public class DataPointToStringConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var asm = Assembly.Load("RealTimeGraphX");
            var type = asm.GetType("RealTimeGraphX.DataPoints." + parameter.ToString());
            var instance = Activator.CreateInstance(type) as IGraphDataPoint;
            return instance.Parse(value.ToString());
        }
    }
}
