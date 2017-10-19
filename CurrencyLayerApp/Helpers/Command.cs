using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyLayerApp.Helpers
{
    class Command:ICommand
    {
        private readonly Action _action;
        public Command(Action action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
