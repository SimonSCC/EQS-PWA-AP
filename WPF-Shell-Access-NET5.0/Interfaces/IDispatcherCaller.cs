using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Shell_Access_NET5._0.Interfaces
{
    public interface IDispatcherCaller
    {
        void InvokeWithUIThread(ICommand command);
    }
}
