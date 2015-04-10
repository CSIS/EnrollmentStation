$rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
$mgm = New-Object "System.Byte[]" 24

$rng.GetByteS($mgm)
$mgmHex = ""
$mgm | foreach { $mgmHex = $mgmHex + $_.ToString("X2") }

Write-Host $mgmHex

[System.IO.File]::AppendAllText("$pwd\ManagementKey", $mgmHex)