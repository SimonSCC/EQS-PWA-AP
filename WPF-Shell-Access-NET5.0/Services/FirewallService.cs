using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET5._0.Services
{
    public class FirewallService
    {
        public FirewallService()
        {

        }

        public void AllowRule()
        {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            
            IEnumerable Rules = (IEnumerable)fwPolicy2.Rules;
            List<dynamic> asList = new List<dynamic>();
            dynamic nodeRule = null;
            foreach (dynamic rule in Rules)
            {
                
                if (rule.Name == "Node.js: Server-side JavaScript")
                {
                    nodeRule = rule;
                }
            }
            if (nodeRule == null)
            {
                //Add the rule
                dynamic newRule = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                newRule.ApplicationName = @"C:\program files\nodejs\node.exe";
                newRule.Action = "NET_FW_ACTION_.NET_FW_ACTION_ALLOW";
                newRule.Description = "Firewall rule added from PWA-ACCESSLocal WPF";
                newRule.Enabled = true;
                newRule.InterfaceTypes = "All";
                newRule.Name = "Node.js: Server-side JavaScript";

                fwPolicy2.Rules.Add(newRule);
            }
            else
            {
                //Check if rule is enabled
                if (nodeRule.Enabled == false)
                {
                    //Add enable the rule
                    nodeRule.Enabled = true;                    
                }
                if (nodeRule.Profiles != 6)
                {
                    nodeRule.Profiles = 6;
                }
            }
        }
    }
}
