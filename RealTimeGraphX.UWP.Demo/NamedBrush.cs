using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP.Demo
{
    public class NamedBrush : DependencyObject
    {
        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }
        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brush", typeof(Brush), typeof(NamedBrush), new PropertyMetadata(null));

        public String Name
        {
            get
            {
                if (Brush is SolidColorBrush)
                {
                    return (Brush as SolidColorBrush).Color.ToString();
                }
                else
                {
                    return "Gradient";
                }
            }
        }
    }
}
