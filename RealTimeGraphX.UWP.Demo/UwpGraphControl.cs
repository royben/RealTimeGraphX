using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace RealTimeGraphX.UWP.Demo
{
    public sealed class UwpGraphControl : Control
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
            DependencyProperty.Register("Controller", typeof(IGraphController), typeof(UwpGraphControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether to display a tool tip with the current cursor value.
        /// </summary>
        public bool DisplayToolTip
        {
            get { return (bool)GetValue(DisplayToolTipProperty); }
            set { SetValue(DisplayToolTipProperty, value); }
        }
        public static readonly DependencyProperty DisplayToolTipProperty =
            DependencyProperty.Register("DisplayToolTip", typeof(bool), typeof(UwpGraphControl), new PropertyMetadata(false));

        public UwpGraphControl()
        {
            this.DefaultStyleKey = typeof(UwpGraphControl);
        }
    }
}
