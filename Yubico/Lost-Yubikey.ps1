. .\SharedFeatures.ps1

$user = Read-Host "Enter username to revoke for"

$fileLog = "$pwd\log.txt"
$logLines = [System.IO.File]::ReadAllLines($fileLog)

Write-Host "Finding previously assigned yubikeys"

$foundAny = $false
foreach ($i in $logLines)
{
    if (!$i.Contains($user))
    {
        continue
    }

    $foundAny = $true
    Write-Host $i
}

if (!$foundAny)
{
    Write-Error "The username was not found in the log"
    return
}

# Confirm
$confirm = Prompt-YesNo -Title "Terminate Yubikey" -Message "Revoke the Yubikeys for $user" -YesText "Yes, revoke" -NoText "No, abort" 

if ($confirm -eq $false)
{
    Write-Error "Aborting, user cancelled"
    return
}

# Re-write log without the lines to be removed
Write-Host "Revoking"
$newLogLines = @()
foreach ($i in $logLines)
{
    if ($i.Contains($id))
    {
        # Skip line
        continue
    }
    
    $newLogLines = $newLogLines + $i

    # Revoke
    Write-Host "TODO: Revoke $i"
}

Write-Host "Updating log"
[System.IO.File]::WriteAllLines($fileLog, $newLogLines)