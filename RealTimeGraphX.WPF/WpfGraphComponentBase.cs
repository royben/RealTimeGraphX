using RealTimeGraphX.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a graph component base class.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    public abstract class WpfGraphComponentBase : Control
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
            DependencyProperty.Register("Controller", typeof(IGraphController), typeof(WpfGraphComponentBase), new PropertyMetadata(null, (d, e) => (d as WpfGraphComponentBase).OnControllerChanged(e.OldValue as IGraphController, e.NewValue as IGraphController)));

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
        protected void InvokeUI(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }
    }
}
