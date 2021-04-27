using System;
using System.Windows.Input;

namespace Traveling_Salesman_Problem.Core.MVVM_Control_Prog
{
    class RelayCommand : ICommand
    {
        private Action<object> exec;
        private Func<object, bool> canExec;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public RelayCommand(Action<object> exc, Func<object,bool> canExc = null)
        {
            exec = exc;
            canExec = canExc;
        }

        public bool CanExecute(object param)
        {
            return canExec == null || canExec(param);
        }

        public void Execute(object param)
        {
            exec(param);
        }
    }
}
