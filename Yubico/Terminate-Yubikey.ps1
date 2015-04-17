. .\SharedFeatures.ps1

$fileLog = "$pwd\log.txt"

$mgmKey = Get-StringSecurely -FileName "$pwd\ManagementKey.bin"

$id = Yubico-GetDeviceId
$logLines = @()

if (Test-Path $fileLog)
{
    $logLines = [System.IO.File]::ReadAllLines($fileLog)
}

Write-Host "Finding previously assigned yubikeys"

$foundAny = $false
foreach ($i in $logLines)
{
    if (!$i.Contains($id))
    {
        continue
    }

    $foundAny = $true
    Write-Host $i
}

if (!$foundAny)
{
    $confirm = Prompt-YesNo -Title "Not found" -Message "The inserted yubikey id was not found in the log, would you like to just reset it?" -YesText "Yes, reset the yubikey" -NoText "No, abort"

    if (!$confirm)
    {
        Write-Error "The inserted yubikey id was not found in the log"
    }
    
    Write-Host "Resetting YubiKey ID: $id"
    Yubico-ResetDevice
    Yubico-SetCHUID
    
    return
}

# Confirm
$confirm = Prompt-YesNo -Title "Terminate Yubikey" -Message "Terminate the yubikey inserted, and revoke the certificate(s)?" -YesText "Yes, reset and revoke" -NoText "No, abort" 

if ($confirm -eq $false)
{
    Write-Error "Aborting, user cancelled"
    return
}

# Re-write log without the lines to be removed
Write-Host "Updating log"
$newLogLines = @()
$serialsToRevoke = @()
foreach ($i in $logLines)
{
    if ($i.Contains("ID: " + $id))
    {
        # Skip line (remove it)
        $serialsToRevoke += $i.Split(";")[3].Trim()

        continue
    }

    $newLogLines = $newLogLines + $i
}

# Revoke
Write-Host "Revoking" $serialsToRevoke.Length "keys"

foreach ($serial in $serialsToRevoke)
{
    Write-Host "Revoking " $serial
    Revoke-Certificate -SerialNumber $serial
}

# Reset Yubikey
Write-Host "Resetting YubiKey ID: $id"
Yubico-ResetDevice
Yubico-SetCHUID

# Write out log
[System.IO.File]::WriteAllLines($fileLog, $newLogLines)