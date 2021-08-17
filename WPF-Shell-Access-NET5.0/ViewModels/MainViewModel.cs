using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Shell_Access_NET;
using WPF_Shell_Access_NET5._0.Commands;
using WPF_Shell_Access_NET5._0.Models;

namespace WPF_Shell_Access_NET5._0.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ShellCommands ShellCommands { get; set; }

        private AccessPointConfiguration _currentConfig;
        public AccessPointConfiguration CurrentConfig { get => _currentConfig; set => SetProperty(ref _currentConfig, value); }

        private PWAInformation _pwaInfo;
        public PWAInformation PWAInfo { get => _pwaInfo; set => SetProperty(ref _pwaInfo, value); }

        //Commands -:: Start

        private readonly DelegateCommand _startHotspotCommand;
        public ICommand StartHotspotCommand => _startHotspotCommand;

        private readonly DelegateCommand _endHotspotCommand;
        public ICommand EndHotspotCommand => _endHotspotCommand;

        private readonly DelegateCommand _clickLink;
        public ICommand ClickLink => _clickLink;

        //Commands -:: End

        public MainViewModel()
        {
            //Commands -:: Start
            _startHotspotCommand = new DelegateCommand(StartHotspot, CanCreateHotspot);
            _endHotspotCommand = new DelegateCommand(EndHotspot, CanEndHotspot);
            _clickLink = new DelegateCommand(OpenLink, CanClickLink);

            //Commands -:: End

            ShellCommands = new ShellCommands();
            InitializeFields();
        }

        private async void InitializeFields()
        {
            PWAInfo = GetPWAInfo();
            CurrentConfig = await GetConfig();
        }

        private PWAInformation GetPWAInfo()
        {
            string ipaddress = "";
            string port = "42100";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipaddress = ip.Address.ToString();
                        }
                    }
                }
            }
            return new PWAInformation(ipaddress, port);  
        }

        private async Task<AccessPointConfiguration> GetConfig()
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

        private async void SetConfig()
        {
            PowerShellScriptResponse resp = await ShellCommands.ChangeProfile(CurrentConfig);

        }
        private async void StartHotspot(object cmdParam)
        {

            if (!CurrentConfig.Equals(await GetConfig()))
            {
                SetConfig();
            }
            PowerShellScriptResponse resp = await ShellCommands.StartHotspot();
            InitializeFields();
            _startHotspotCommand.InvokeCanExecuteChanged();

        }
        private async void EndHotspot(object cmdParam)
        {
            if (!CurrentConfig.Equals(await GetConfig()))
            {
                SetConfig();
            }
            PowerShellScriptResponse resp = await ShellCommands.StopHotspot();
            InitializeFields();
            _endHotspotCommand.InvokeCanExecuteChanged();

        }

        private void OpenLink(object cmdParam)
        {
            ShellCommands.StartBrowser(PWAInfo.LinkToPWA);
            _clickLink.InvokeCanExecuteChanged();

        }

        private bool CanCreateHotspot(object cmdParam)
        {
            PowerShellScriptResponse Response = ShellCommands.GetOperationalState();
            if (Response.Response.Contains("Off"))
            {
                return true;
            }
            else if (Response.Response.Contains("On"))
            {
                return false;
            }
            else
            {
                throw new Exception("An unexpected error occured" + "\n" + Response.Error);
            } 
        }

        private bool CanEndHotspot(object cmdParam)
        {
            PowerShellScriptResponse Response = ShellCommands.GetOperationalState();
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


        private bool CanClickLink(object cmdParam)
        {
            return true;
        }
    }
}
