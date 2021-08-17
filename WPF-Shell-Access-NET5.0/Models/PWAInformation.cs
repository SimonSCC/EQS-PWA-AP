using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET5._0.Models
{
    public class PWAInformation
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string LinkToPWA { get; set; }

        public PWAInformation(string ip, string port)
        {
            IP = ip;
            Port = port;

            if (IP == null || IP == "")
            {
                LinkToPWA = null;
            }
            else
            {
                LinkToPWA = $"https://{ip}:{port}";
            }
        }
    }
}
