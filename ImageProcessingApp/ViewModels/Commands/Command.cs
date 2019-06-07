using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageProcessingApp.ViewModels.Commands
{
    public class Command : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;
        public Command(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _action();
        }
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
