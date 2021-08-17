using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Shell_Access_NET5._0.Services;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace WPF_Shell_Access_NET5._0.Commands
{
    public class StopHotspotCommand : AsyncCommandBase
    {
        private MainViewModel MainViewModel { get; set; }
        private HotspotService HotSpotServ { get; set; }

        public StopHotspotCommand(MainViewModel mainViewModel, HotspotService hotSpotServ)
        {
            this.MainViewModel = mainViewModel;
            this.HotSpotServ = hotSpotServ;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            MainViewModel.HotSpotStatusMessage = "Stopping hotspot...";
            await HotSpotServ.EndHotspot();
            MainViewModel.UpdateFields();
        }
    }
}
