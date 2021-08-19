using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET5._0.Models
{
    public class PWAInformation
    {
        public List<string> WifiNetworkIps { get; set; }
        public string Port { get; set; }
        public List<string> PotentialLinksToPWA { get; set; }

        public PWAInformation(List<string> ips, string port)
        {
            WifiNetworkIps = ips;
            Port = port;

            if (WifiNetworkIps == null || WifiNetworkIps.Count == 0)
            {
                PotentialLinksToPWA = null;
            }
            else
            {
                PotentialLinksToPWA = new List<string>();
                foreach (string ip in WifiNetworkIps)
                {
                    PotentialLinksToPWA.Add($"http://{ip}:{port}");
                }
            }
        }
    }
}
