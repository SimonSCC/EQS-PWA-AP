using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET5._0.Models
{
    public class PowerShellScriptResponse
    {
        public string Response { get; set; }
        public string Error { get; set; }

        public PowerShellScriptResponse(string resp, string err)
        {
            Response = resp;
            Error = err;
        }
    }
}
