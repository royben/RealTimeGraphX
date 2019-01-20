using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace RealTimeGraphX.UWP.Demo
{
    public class NamedColor : DependencyObject
    {
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(NamedColor), new PropertyMetadata(null));

        public String Name
        {
            get { return Color.ToString(); }
        }
    }
}
