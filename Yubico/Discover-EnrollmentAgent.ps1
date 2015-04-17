. .\SharedFeatures.ps1

$certificates = Get-ChildItem Cert:\CurrentUser\My

foreach ($cert in $certificates)
{
    # Filter to Enhanced Key Usage extension
    $eku = [System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension]($cert.Extensions | where { $_.Oid.Value -eq "2.5.29.37" })[0]

    # Take only the Certificate Request Agent usage
    $usg = ($eku.EnhancedKeyUsages | where { $_.Value -eq "1.3.6.1.4.1.311.20.2.1" }) -ne $null

    if ($usg)
    {
        Write-Host "Found a certificate, expiry " $cert.NotAfter "(" $cert.NotAfter.Kind ")"
        $cert.Thumbprint
        $cert.Subject
        
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