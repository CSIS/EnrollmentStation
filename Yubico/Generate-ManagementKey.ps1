. .\SharedFeatures.ps1

$mgmFile = "$pwd\ManagementKey.bin"

Write-Host "Generating key"
$mgmKey = Generate-RandomStringHex -LengthBytes 24

Write-Host "Writing out key"
Store-StringSecurely -FileName $mgmFile -Text $mgmKey