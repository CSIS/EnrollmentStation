. .\SharedFeatures.ps1

$fileEa = "$pwd\enrollmentagent.txt"
$current = Get-String -FileName $fileEa -FailFast $false

$certificates = Get-ChildItem Cert:\CurrentUser\My

$thumbprints = @()
foreach ($cert in $certificates)
{
    # Filter to Enhanced Key Usage extension
    $eku = [System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension]($cert.Extensions | where { $_.Oid.Value -eq "2.5.29.37" })[0]

    # Take only the Certificate Request Agent usage
    $usg = ($eku.EnhancedKeyUsages | where { $_.Value -eq "1.3.6.1.4.1.311.20.2.1" }) -ne $null

    if ($usg)
    {
        $tmpNumber = $thumbprints.Count + 1
        $thumbprints += $cert.Thumbprint
        
        if ($current -eq $cert.Thumbprint)
        {
            Write-Host "Certificate " -NoNewline
            Write-Host -ForegroundColor Green $tmpNumber -NoNewline
            Write-Host -ForegroundColor Yellow " This is the currently chosen certificate"
        }
        else
        {
            Write-Host "Certificate " -NoNewline
            Write-Host -ForegroundColor Green $tmpNumber
        }

        Write-Host "Found a certificate, expiry " $cert.NotAfter "(" $cert.NotAfter.Kind ")"
        Write-Host $cert.Thumbprint
        Write-Host "Subject: " $cert.Subject
        Write-Host "Issuer : " $cert.Issuer
        
        Write-Host "Private key: " -NoNewline
        if ($cert.HasPrivateKey)
        {
            Write-Host -ForegroundColor Green "Yes"
        }
        else
        {
            Write-Host -ForegroundColor Red "No"
        }

        Write-Host ""
    }
}

Write-Host "Which certificate would you like to use?"
Write-Host "Enter " -NoNewline
Write-Host -ForegroundColor Green "0" -NoNewline
Write-Host " to exit, or " -NoNewline
Write-Host -ForegroundColor Green "1 -" $thumbprints.Count -NoNewline
Write-Host " to choose a certificate"

$number = [int](Read-Host)
if ($number -le 0 -or $number -gt $thumbprints.Count)
{
    return
}

Store-String -FileName $fileEa -Text $thumbprints[$number - 1]