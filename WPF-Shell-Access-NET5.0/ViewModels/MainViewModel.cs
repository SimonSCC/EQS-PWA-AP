using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Shell_Access_NET;
using WPF_Shell_Access_NET5._0.Commands;
using WPF_Shell_Access_NET5._0.Interfaces;
using WPF_Shell_Access_NET5._0.Models;
using WPF_Shell_Access_NET5._0.Services;

namespace WPF_Shell_Access_NET5._0.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ShellCommands ShellCommands { get; set; }
        public HotspotService HotspotService { get; set; }

        private string _minutesBeforeStoppingHotspot;
        public string MinutesBeforeStoppingHotspot { get => _minutesBeforeStoppingHotspot; set => SetProperty(ref _minutesBeforeStoppingHotspot, value); }
        public bool IsOn { get; set; }
        public IDispatcherCaller UIThreadCaller { get; set; }

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
            new FirewallService().AllowRule();
            ShellCommands = new ShellCommands();
            HotspotService = new HotspotService(this, ShellCommands);
            MinutesBeforeStoppingHotspot = "60";
            //Commands Begin
            StartHotspotCommand = new StartHotspotCommand(this, HotspotService);
            StopHotspotCommand = new StopHotspotCommand(this, HotspotService);
            HyperlinkOpenCommand = new HyperlinkOpenCommand(this, HotspotService);


            //Commands -:: End
            UpdateFields();


        }

        public void CountDown()
        {
            try
            {
                while (!IsOn)
                {
                    Thread.Sleep(10000);
                }
                while (IsOn)
                {
                    Thread.Sleep(60000);
                    int intVal = int.Parse(MinutesBeforeStoppingHotspot);
                    intVal = intVal -= 1;
                    if (intVal <= 0 || !IsOn)
                    {
                        return;
                    }
                    MinutesBeforeStoppingHotspot = intVal.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }           
        }
      

        public async void UpdateFields()
        {
            PWAInfo = GetPWAInfo();
            CurrentConfig = await HotspotService.GetConfig();
            IsOn = await HotspotService.IsHotspotOn();
            if (IsOn)
            {
                HotSpotStatusMessage = "A hotspot is running!";
                StartHotspotBtnEnabled = false;
                StopHotspotBtnEnabled = true;
            }
            else if (!IsOn)
            {
                HotSpotStatusMessage = "A hotspot is not running!";
                StartHotspotBtnEnabled = true;
                StopHotspotBtnEnabled = false;
            }            
        }

        private PWAInformation GetPWAInfo()
        {
            List<string> potentialIpAddresses = new List<string>();
            string port = "4200";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            potentialIpAddresses.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return new PWAInformation(potentialIpAddresses, port);
        }
    }
}
