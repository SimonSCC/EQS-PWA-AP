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
using WPF_Shell_Access_NET5._0.Services;

namespace WPF_Shell_Access_NET5._0.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ShellCommands ShellCommands { get; set; }
        public HotspotService HotspotService { get; set; }
        
        
        private string _hotspotStatusMessage;
        public string HotSpotStatusMessage { get => _hotspotStatusMessage; set => SetProperty(ref _hotspotStatusMessage, value); }


        private AccessPointConfiguration _currentConfig;
        public AccessPointConfiguration CurrentConfig { get => _currentConfig; set => SetProperty(ref _currentConfig, value); }

        private PWAInformation _pwaInfo;
        public PWAInformation PWAInfo { get => _pwaInfo; set => SetProperty(ref _pwaInfo, value); }

        //Commands -:: Start

        public ICommand StartHotspotCommand { get; }

        private bool _startHotspotBtnEnabled;
        public bool StartHotspotBtnEnabled { get => _startHotspotBtnEnabled; set => SetProperty(ref _startHotspotBtnEnabled, value); }


        public ICommand StopHotspotCommand { get; }

        private bool __stopHotspotBtnEnabled;
        public bool StopHotspotBtnEnabled { get => __stopHotspotBtnEnabled; set => SetProperty(ref __stopHotspotBtnEnabled, value); }

        public ICommand HyperlinkOpenCommand { get; }

        //Commands -:: End


        public MainViewModel()
        {
            ShellCommands = new ShellCommands();
            HotspotService = new HotspotService(this, ShellCommands);

            //Commands Begin
            StartHotspotCommand = new StartHotspotCommand(this, HotspotService);
            StopHotspotCommand = new StopHotspotCommand(this, HotspotService);
            HyperlinkOpenCommand = new HyperlinkOpenCommand(HotspotService);


            //Commands -:: End
            UpdateFields();
        }



        public async void UpdateFields()
        {
            PWAInfo = GetPWAInfo();
            CurrentConfig = await HotspotService.GetConfig();
            bool isOn = await HotspotService.IsHotspotOn();
            if (isOn)
            {
                HotSpotStatusMessage = "A hotspot is running!";
                StartHotspotBtnEnabled = false;
                StopHotspotBtnEnabled = true;
            }
            else if (!isOn)
            {
                HotSpotStatusMessage = "A hotspot is not running!";
                StartHotspotBtnEnabled = true;
                StopHotspotBtnEnabled = false;
            }            
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
    }
}
