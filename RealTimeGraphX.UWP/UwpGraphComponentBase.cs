using RealTimeGraphX.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RealTimeGraphX.UWP
{
    /// <summary>
    /// Represents a graph component base class.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    public class UwpGraphComponentBase : Control
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
            DependencyProperty.Register("Controller", typeof(IGraphController), typeof(UwpGraphComponentBase), new PropertyMetadata(null,(d,e) => (d as UwpGraphComponentBase).OnControllerChanged(e.OldValue as IGraphController, e.NewValue as IGraphController)));


        /// <summary>
        /// Called when the controller has changed.
        /// </summary>
        /// <param name="oldController">The old controller.</param>
        /// <param name="newController">The new controller.</param>
        protected virtual void OnControllerChanged(IGraphController oldController, IGraphController newController)
        {
            if (oldController != null)
            {
                oldController.VirtualRangeChanged -= OnVirtualRangeChanged;
            }

            if (newController != null)
            {
                newController.VirtualRangeChanged += OnVirtualRangeChanged;
            }
        }

        /// <summary>
        /// Handles the <see cref="IGraphController.VirtualRangeChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnVirtualRangeChanged(object sender, RangeChangedEventArgs e)
        {
            //Optional
        }

        /// <summary>
        /// Invokes the specified method on the component dispatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        protected async void InvokeUI(Action action)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { action(); });
        }
    }
}
