[Windows.System.UserProfile.LockScreen,Windows.System.UserProfile,ContentType=WindowsRuntime] | Out-Null

Add-Type -AssemblyName System.Runtime.WindowsRuntime
$asTaskGeneric = ([System.WindowsRuntimeSystemExtensions].GetMethods() | ? { $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncOperation`1' })[0]
Function Await($WinRtTask, $ResultType) {
    $asTask = $asTaskGeneric.MakeGenericMethod($ResultType)
    $netTask = $asTask.Invoke($null, @($WinRtTask))
    $netTask.Wait(-1) | Out-Null
    $netTask.Result
}
Function AwaitAction($WinRtAction) {
    $asTask = ([System.WindowsRuntimeSystemExtensions].GetMethods() | ? { $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and !$_.IsGenericMethod })[0]
    $netTask = $asTask.Invoke($null, @($WinRtAction))
    $netTask.Wait(-1) | Out-Null
}

$connectionProfile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetInternetConnectionProfile()
$tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile($connectionProfile)

#If to want to set the SSID and Passphrase for the hostpot, get the current settings 

$accessPointConfiguration = $tetheringManager.GetCurrentAccessPointConfiguration()
#then update the configuration

$accessPointConfiguration.Ssid = '?MYSSIDHERE?'

$accessPointConfiguration.Passphrase = '?PASSPHRASEHERE?'

#check whether it's active using
$tetherinManager.GetOperationalStatus

#Stop the hotspot if necessary then set the new configuration using

AwaitAction ($tetheringManager.ConfigureAccessPointAsync($accessPointConfiguration)) ([Windows.Networking.NetworkOperators.NetworkOperatorTetheringOperationResult])

#then start the hotspot again

#and, if you want to stop the Hotspot turning off when there are no clients attached, set this registry value

#New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\icssvc\Settings" -Name "PeerlessTimeoutEnabled" -Value 0 -Type "DWord"

#You may need to restart the Internet Connection Service (isssvc) for this to take effect