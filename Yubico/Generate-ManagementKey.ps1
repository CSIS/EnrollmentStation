. .\SharedFeatures.ps1

$mgmFile = "$pwd\ManagementKey.bin"

$useYubicHsm = Prompt-YesNo -Title "Use YubicoHSM" -Message "Use a Yubico HSM to generate entropy?" -YesText "Yes, use HSM" -NoText "No, use Windows RNG" 

Write-Host "Generating key"
$mgmKey = Generate-RandomStringHex -LengthBytes 24 -UseYubicoHsm $useYubicHsm

Write-Host "Writing out key"
Store-StringSecurely -FileName $mgmFile -Text $mgmKey