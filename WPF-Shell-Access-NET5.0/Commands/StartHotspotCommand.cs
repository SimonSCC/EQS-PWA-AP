using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace WPF_Shell_Access_NET5._0.Commands
{
    public class StartHotspotCommand : AsyncCommandBase
    {
        public MainViewModel MainViewModel { get; set; }

        public StartHotspotCommand(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        protected override Task ExecuteAsync(object parameter)
        {
            
        }
    }
}
