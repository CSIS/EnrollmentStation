. .\SharedFeatures.ps1

# TODO: Enrollment Agent thumbprint
$enrollmentThumbprint = "89A22802E373A986C9961D414422A873B912B05E"

$mgmKey = Get-StringSecurely -FileName "$pwd\ManagementKey.bin"

$useYubicHsm = Prompt-YesNo -Title "Use YubicoHSM" -Message "Use a Yubico HSM to generate entropy?" -YesText "Yes, use HSM" -NoText "No, use Windows RNG" 

$newPin = Request-SecurePassword -Question "Set new PIN (max 8 characters)"
$newPuk = Generate-RandomString -Length 8 -UseYubicoHsm $useYubicHsm
$newUser = Read-Host "Enter username to Enroll for (Domain\User)"

$fileLog = "$pwd\log.txt"
$filePublicKey = "$pwd\public.pem"
$fileCsr = "$pwd\request.csr"
$fileCert = "$pwd\cert.crt"

$id = Yubico-GetDeviceId
sleep 5

Write-Host "Resetting YubiKey ID: $id"
Yubico-ResetDevice

Write-Host "Setting Management Key"
Yubico-SetManagementKey -NewManagementKey $mgmKey

Write-Host "Setting CHUID"
Yubico-SetCHUID -ManagementKey $mgmKey

Write-Host "Setting PIN"
Yubico-SetPin -ManagementKey $mgmKey -NewPin $newPin

Write-Host "Setting PUK"
Yubico-SetPuk -ManagementKey $mgmKey -NewPuk $newPuk

Write-Host "Generating new private key"
Yubico-GenerateKey -ManagementKey $mgmKey -OutputFile $filePublicKey

Write-Host "Generating new CSR"

Yubico-GenerateCSR -Pin $newPin -PublicKey $filePublicKey -RequestFile $fileCsr

Write-Host "Signing key"
Sign-OnBehalfOf -EnrollmentAgentCert $enrollmentThumbprint -User $newUser -RequestFile $fileCsr -CertificateFile $fileCert

Write-Host "Setting cert"
Yubico-Importcert -ManagementKey $mgmKey -CertificateFile $fileCert

Write-Host "Logging configuration"

$line = "ID: $id; User: $newUser; PUK: $newPuk"

[System.IO.File]::AppendAllText($fileLog, "$line`n")

Write-Host "Clearing intermediate files"

Remove-ItemIfExists $filePublicKey
Remove-ItemIfExists $fileCsr
Remove-ItemIfExists $fileCert