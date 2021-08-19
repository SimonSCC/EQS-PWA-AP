using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Shell_Access_NET5._0.Services;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace WPF_Shell_Access_NET5._0.Commands
{
    public class HyperlinkOpenCommand : AsyncCommandBase
    {
        public HotspotService HotSpotService { get; }

        public HyperlinkOpenCommand(MainViewModel vm, HotspotService hotSpotService)
        {
            HotSpotService = hotSpotService;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            HotSpotService.OpenLink(parameter.ToString());
        }
    }
}
