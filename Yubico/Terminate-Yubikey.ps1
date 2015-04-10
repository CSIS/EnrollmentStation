. .\SharedFeatures.ps1

$fileLog = "$pwd\log.txt"

$mgmKey = Get-StringSecurely -FileName "$pwd\ManagementKey.bin"

$id = Yubico-GetDeviceId
$logLines = [System.IO.File]::ReadAllLines($fileLog)

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
    Write-Error "The inserted yubikey id was not found in the log"
    return
}

# Confirm
$confirm = Prompt-YesNo -Title "Terminate Yubikey" -Message "Terminate the yubikey inserted?" -YesText "Yes, remove it" -NoText "No, abort" 

if ($confirm -eq $false)
{
    Write-Error "Aborting, user cancelled"
    return
}

# Re-write log without the lines to be removed
Write-Host "Updating log"
$newLogLines = @()
foreach ($i in $logLines)
{
    if ($i.Contains($id))
    {
        # Skip line
        continue
    }

    $newLogLines = $newLogLines + $i
}

[System.IO.File]::WriteAllLines($fileLog, $newLogLines)

# Reset Yubikey
Write-Host "Resetting device"
Yubico-ResetDevice