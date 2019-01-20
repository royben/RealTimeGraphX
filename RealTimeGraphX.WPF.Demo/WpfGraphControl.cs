using System;
using System.Collections.Generic;
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

namespace RealTimeGraphX.WPF.Demo
{
    public class WpfGraphControl : Control
    {
        /// <summary>
        /// Gets or sets the graph controller.
        /// </summary>
        public IGraphController Controller
        {
            get { return (IGraphController)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IGraphController), typeof(WpfGraphControl), new PropertyMetadata(null));

        /// <summary>
        /// Initializes the <see cref="WpfGraphControl"/> class.
        /// </summary>
        static WpfGraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfGraphControl), new FrameworkPropertyMetadata(typeof(WpfGraphControl)));
        }
    }
}
