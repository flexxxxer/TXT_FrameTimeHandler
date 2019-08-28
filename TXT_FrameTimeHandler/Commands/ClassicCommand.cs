using System;
using System.Windows.Input;

namespace TXT_FrameTimeHandler.Commands
{
    public class ClassicCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public ClassicCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public ClassicCommand(Action<object> execute) : this(execute, null) { }

        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
            => this._canExecute is null ? true : this._canExecute.Invoke(parameter);

        public void Execute(object parameter)
            => this._execute(parameter);

        public void RaiseCanExecuteChanged() 
            => CommandManager.InvalidateRequerySuggested();
    }
}