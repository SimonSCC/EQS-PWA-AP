using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Shell_Access_NET5._0.Services;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace WPF_Shell_Access_NET5._0.Commands
{
    public class StartHotspotCommand : AsyncCommandBase
    {
        public MainViewModel MainViewModel { get; set; }
        public HotspotService HotSpotService { get; }
        public StartHotspotCommand(MainViewModel mainViewModel, HotspotService hotSpotService)
        {
            MainViewModel = mainViewModel;
            HotSpotService = hotSpotService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            MainViewModel.HotSpotStatusMessage = "Initializing hotspot...";
            await HotSpotService.StartHotspot();
            MainViewModel.UpdateFields();
        }
    }
}
