using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents a graph relay command.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class GraphCommand : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphCommand"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="canExecute">The can execute.</param>
        public GraphCommand(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphCommand"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public GraphCommand(Action action) : this(action, null)
        {

        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }

            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _action();
        }

        /// <summary>
        /// Raises the can execute changed event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        /// <returns></returns>
        public event EventHandler CanExecuteChanged;
    }
}
