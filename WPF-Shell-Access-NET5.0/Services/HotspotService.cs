using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Shell_Access_NET;
using WPF_Shell_Access_NET5._0.Models;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace WPF_Shell_Access_NET5._0.Services
{
    public class HotspotService
    {
        private MainViewModel MainViewModel { get; }
        private ShellCommands ShellCommands { get; }

        public HotspotService(MainViewModel mainViewModel, ShellCommands shellCommands)
        {
            this.MainViewModel = mainViewModel;
            ShellCommands = shellCommands;
        }

        public async Task StartHotspot()
        {
            if (!MainViewModel.CurrentConfig.Equals(await GetConfig()))
            {
                SetConfig();
            }
            PowerShellScriptResponse resp = await ShellCommands.StartHotspot();
        }

        public async Task EndHotspot()
        {
            if (!MainViewModel.CurrentConfig.Equals(await GetConfig()))
            {
                SetConfig();
            }
            PowerShellScriptResponse resp = await ShellCommands.StopHotspot();
        }

        public async void SetConfig()
        {
            PowerShellScriptResponse resp = await ShellCommands.ChangeProfile(MainViewModel.CurrentConfig);

        }

        public async Task<AccessPointConfiguration> GetConfig()
        {
            PowerShellScriptResponse result = await ShellCommands.GetProfile();
            string rawString = result.Response;
            if (rawString != null && rawString != "")
            {
                string[] split = rawString.Split("\n");
                return new AccessPointConfiguration(split[0], split[1]);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsHotspotOn()
        {
            PowerShellScriptResponse Response = await ShellCommands.GetOperationalState();
            if (Response.Response.Contains("Off"))
            {
                return false;
            }
            else if (Response.Response.Contains("On"))
            {
                return true;
            }
            else
            {
                throw new Exception("An unexpected error occured" + "\n" + Response.Error);
            }
        }

        public void OpenLink()
        {
            ShellCommands.StartBrowser(MainViewModel.PWAInfo.LinkToPWA);
        }

    }
}
