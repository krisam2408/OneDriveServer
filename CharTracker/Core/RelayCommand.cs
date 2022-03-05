using System;
using System.Windows.Input;

namespace RetiraTracker.Core
{
    public class RelayCommand:ICommand
    {
        private readonly Action<object> m_execute;
        private readonly Func<object, bool> m_canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null || m_canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            m_execute(parameter);
        }
    }
}
