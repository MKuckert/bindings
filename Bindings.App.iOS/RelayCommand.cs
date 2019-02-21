using System;
using System.Windows.Input;

namespace Bindings.App.iOS
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _callback;

        public RelayCommand(Action<object> callback)
        {
            _callback = callback;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _callback(parameter);
    }
}
