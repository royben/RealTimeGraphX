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
            DependencyProperty.Register("Controller", typeof(IGraphController), typeof(UwpGraphControl), new PropertyMetadata(null,(d,e) => (d as UwpGraphControl).OnControllerChanged(e.OldValue as IGraphController, e.NewValue as IGraphController)));

        private void OnControllerChanged(IGraphController graphController1, IGraphController graphController2)
        {
            
        }

        public UwpGraphControl()
        {
            this.DefaultStyleKey = typeof(UwpGraphControl);
        }
    }
}
