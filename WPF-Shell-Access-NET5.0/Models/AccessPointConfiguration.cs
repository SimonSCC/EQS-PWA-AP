using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET5._0.Models
{
    public class AccessPointConfiguration
    {
        public string SSID { get; set; }
        public string PassPhrase { get; set; }

        public AccessPointConfiguration(string ssid, string passphrase)
        {
            SSID = ssid;
            PassPhrase = passphrase;
        }

        public override bool Equals(object obj)
        {
            AccessPointConfiguration newVal = (AccessPointConfiguration)obj;
            if (newVal.SSID == this.SSID && newVal.PassPhrase == this.PassPhrase)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
    }
}
