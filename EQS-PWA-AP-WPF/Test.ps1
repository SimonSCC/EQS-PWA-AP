
param([string]$a)

write-host $a

$tetheringManager.TetheringOperationalState



[hashtable]$Return = @{}
$Return.ReturnCode = [int]1
$Return.ReturnString = [string]"All Done!"
return $Return