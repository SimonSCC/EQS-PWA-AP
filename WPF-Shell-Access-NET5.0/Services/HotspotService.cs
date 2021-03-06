using System;
using System.Threading;
using System.Threading.Tasks;
using WPF_Shell_Access_NET;
using WPF_Shell_Access_NET5._0.Models;
using WPF_Shell_Access_NET5._0.ViewModels;
using System.Timers;


namespace WPF_Shell_Access_NET5._0.Services
{
    public class HotspotService
    {
        private MainViewModel MainViewModel { get; }
        private ShellCommands ShellCommands { get; }
        public System.Timers.Timer CountdownToStopHotspot { get; set; }


        public HotspotService(MainViewModel mainViewModel, ShellCommands shellCommands)
        {
            this.MainViewModel = mainViewModel;
            ShellCommands = shellCommands;
        }

        public void StartTimer(int countdownToEndHotSpot)   
        {            
            CountdownToStopHotspot = new System.Timers.Timer(countdownToEndHotSpot * 60000);
            CountdownToStopHotspot.Elapsed += OnTimedEvent;
            CountdownToStopHotspot.Enabled = true;
        }
        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (await IsHotspotOn())
                {
                    MainViewModel.UIThreadCaller.InvokeWithUIThread(MainViewModel.StopHotspotCommand);
                }
            }
            catch (Exception err)
            {
                MainViewModel.UIThreadCaller.InvokeWithUIThread(MainViewModel.StopHotspotCommand);
                Console.WriteLine(err.Message);
            }
           
        }

        public async Task StartHotspot()
        {
            if (!MainViewModel.CurrentConfig.Equals(await GetConfig()))
            {
                SetConfig();
            }
            PowerShellScriptResponse resp = await ShellCommands.StartHotspot();
            int countdownToEndHotSpot = 60;
            int.TryParse(MainViewModel.MinutesBeforeStoppingHotspot, out countdownToEndHotSpot);
            StartTimer(countdownToEndHotSpot);

        }

        public async Task EndHotspot()
        {
            if (!MainViewModel.CurrentConfig.Equals(await GetConfig()))
            {
                SetConfig();
            }
            CountdownToStopHotspot.Stop();
            CountdownToStopHotspot.Dispose();
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

        public void OpenLink(string v)
        {
            ShellCommands.StartBrowser(v);
        }

    }
}
