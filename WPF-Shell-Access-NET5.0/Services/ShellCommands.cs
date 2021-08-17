using System;
using System.IO;
using System.Diagnostics;
using WPF_Shell_Access_NET5._0.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPF_Shell_Access_NET
{
    public class ShellCommands
    {
        public void StartBrowser(string uri)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = uri;
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);

        }

        async Task<PowerShellScriptResponse> ExecutePowerShellScriptAsync(string path)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = path;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            return new PowerShellScriptResponse(await process.StandardOutput.ReadToEndAsync(), await process.StandardError.ReadToEndAsync());
        }

        public async Task<PowerShellScriptResponse> StartHotspot()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\StartHotspot.ps1";
            return await ExecutePowerShellScriptAsync(path);
        }

        public async Task<PowerShellScriptResponse> StopHotspot()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\StopHotspot.ps1";
            return await ExecutePowerShellScriptAsync(path);
        }

        public async Task<PowerShellScriptResponse> ChangeProfile(AccessPointConfiguration newConfig)
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\SetAccessPointConfiguration.ps1";
            string[] allLines = await File.ReadAllLinesAsync(path);
            string[] original = allLines;

            for (int i = 0; i < allLines.Length; i++)
            {
                if (allLines[i].Contains("?MYSSIDHERE?"))
                {
                    allLines[i] = allLines[i].Replace("?MYSSIDHERE?", newConfig.SSID);
                }
                if (allLines[i].Contains("?PASSPHRASEHERE?"))
                {
                    allLines[i] = allLines[i].Replace("?PASSPHRASEHERE?", newConfig.PassPhrase);
                }
            }

            File.WriteAllLines(path, allLines);
            PowerShellScriptResponse response = await ExecutePowerShellScriptAsync(path);
            File.WriteAllLines(path, original);
            return response;
        }
        public async Task<PowerShellScriptResponse> GetProfile()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\GetAccessPointConfiguration.ps1";
            return await ExecutePowerShellScriptAsync(path);
        }

        public async Task<PowerShellScriptResponse> GetOperationalState()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\GetOperationalState.ps1";
            return await ExecutePowerShellScriptAsync(path);
        }
    }
}
