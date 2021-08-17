using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Shell_Access_NET5._0.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        protected abstract Task ExecuteAsync(object parameter);
      
    }
}
