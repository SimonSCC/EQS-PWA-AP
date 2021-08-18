using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET5._0.Models
{
    public class AccessPointConfiguration
    {
        private string ssid;

        public string SSID
        {
            get { return ssid; }
            set
            {
                if (!String.IsNullOrEmpty(value.Trim()))
                {
                    ssid = value;
                }
            }
        }

        private string passPhrase;

        public string PassPhrase
        {
            get { return passPhrase; }
            set
            {
                if (!String.IsNullOrEmpty(value.Trim()))
                {
                    if (value.Trim().Length >= 8)
                    {
                        passPhrase = value;
                    }
                }

            }
        }




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
