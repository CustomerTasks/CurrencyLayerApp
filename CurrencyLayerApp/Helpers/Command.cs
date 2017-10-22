using System;
using System.Windows.Input;

namespace CurrencyLayerApp.Helpers
{
    /// <inheritdoc />
    /// <summary>
    /// Base declaration for implementing commands
    /// </summary>
    class CommandBase:ICommand
    {
        /// <summary>
        /// Some task which command executes
        /// </summary>
        private readonly Action _action;
        public CommandBase(Action action)
        {
            _action = action;
        }
        public void Execute(object parameter)
        {
            _action();
        }
        #region <Additional>

        public bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
