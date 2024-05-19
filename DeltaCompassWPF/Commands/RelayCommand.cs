using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeltaCompassWPF.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _Execute {  get; set; }
        private Predicate<object> _CanExecute { get; set; }

        public RelayCommand(Action<object> ExecuteMethod)
        {
            _Execute = ExecuteMethod;
            _CanExecute = null;
        }

        public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod) 
        {
            _Execute = ExecuteMethod;
            _CanExecute = CanExecuteMethod;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) 
        {
            return _CanExecute==null?true:_CanExecute(parameter);
        }

        public void Execute(object parameter) 
        {
            _Execute(parameter);
        }
    }
}
