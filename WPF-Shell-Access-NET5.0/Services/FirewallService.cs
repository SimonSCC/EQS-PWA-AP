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
                ////Add the rule
                //dynamic newRule = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                //newRule.ApplicationName = @"C:\program files\nodejs\node.exe";
                //newRule.Action = "NET_FW_ACTION_.NET_FW_ACTION_ALLOW";
                //newRule.Description = "Firewall rule added from PWA-ACCESSLocal WPF";
                //newRule.Enabled = true;
                //newRule.InterfaceTypes = "All";
                //newRule.Name = "Node.js: Server-side JavaScript";

                //fwPolicy2.Rules.Add(newRule);
                Console.WriteLine("Add Rule");
                //If using vs code most likely there's already a rule
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
                    //Makes the profile 6, which means allow both from public and private network. the hotspot is public, so, it's required.
                    nodeRule.Profiles = 6;
                    //Doing this does so that i can't delete the rule "Node.js: Server-side JavaScript". Only from cmd prompt I can delete it by saying:
                    //netsh advfirewall firewall delete rule name="Node.js: Server-side JavaScript"
                }
            }
        }
    }
}
