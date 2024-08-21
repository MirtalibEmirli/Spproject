using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spproject.Commands
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
 

    public class RelayCommand : ICommand
    {
        private Action<object?>? _execute;
        public Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?>? execute = null, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute is null)
                return true;
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if (_execute is not null)
                _execute(parameter);
        }
    }

}
