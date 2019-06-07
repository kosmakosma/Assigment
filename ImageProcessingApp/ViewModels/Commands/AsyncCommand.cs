using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageProcessingApp.ViewModels.Commands
{

    public abstract class AsyncCommandBase : ICommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);

        protected bool _isTaskFinished;

        public async void Execute(object parameter)
        {
            _isTaskFinished = false;
            RaiseCanExecuteChanged();
            await ExecuteAsync(parameter);
            _isTaskFinished = true;
            RaiseCanExecuteChanged();
        }
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _command;
        private Func<bool> _canExecute;
        public AsyncCommand(Func<Task> command, Func<bool> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
            _isTaskFinished = true;
        }
        public override bool CanExecute(object parameter)
        {
            return _canExecute() && _isTaskFinished;
        }
        public override Task ExecuteAsync(object parameter)
        {
            return _command();
        }
    }

}
