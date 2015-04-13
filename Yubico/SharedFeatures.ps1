function Generate-RandomString
{
    # http://www.peterprovost.org/blog/2007/06/22/Quick-n-Dirty-PowerShell-Password-Generator/
    param ( 
        [int] $Length = 12,
        [string] $Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678"
    )

    $bytes = new-object "System.Byte[]" $Length
    $rnd = new-object System.Security.Cryptography.RNGCryptoServiceProvider
    $rnd.GetBytes($bytes)
    $result = ""
    for( $i=0; $i -lt $Length; $i++ )
    {
        $result += $Characters[ $bytes[$i] % $Characters.Length ]	
    }
    $result
}

function Generate-RandomStringHex
{
    param ( 
        [int] $LengthBytes = 12,
        [bool] $UseYubicoHsm = $false
    )

    if ($UseYubicoHsm -eq $true)
    {
        $p = Start-Process .\bin\HSMRNG.exe $LengthBytes -NoNewWindow -Wait -RedirectStandardOutput tmp -PassThru
        $string = Get-Content tmp
        Remove-ItemIfExists tmp

        if ($p.ExitCode -ne 0)
        {
	        throw "Error getting random data. Missing Yubico HSM? Returncode was " + $p.ExitCode
        }

        $string
    }
    else
    {
        $bytes = new-object "System.Byte[]" $LengthBytes
        $rnd = new-object System.Security.Cryptography.RNGCryptoServiceProvider
        $rnd.GetBytes($bytes)
    
        $string = ""
        $bytes | foreach { $string = $string + $_.ToString("X2") }

        $string
    }
}

function Request-SecurePassword
{
    param ( 
        [string] $Question = "Enter a password",
        [int] $MaxLength = 256
    )

    $password = Read-Host $Question -AsSecureString
    $password = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))

    if ($password.Length -gt $MaxLength)
    {
	    Write-Error "PIN must be at most $MaxLength characters"
	    return
    }

    $password
}

function Yubico-ResetDevice
{
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a verify-pin -P RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a verify-pin -P RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a verify-pin -P RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a verify-pin -P RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a change-puk -P RNADOMSI -N RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a change-puk -P RNADOMSI -N RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a change-puk -P RNADOMSI -N RNADOMSI" -Wait -NoNewWindow
    Start-Process .\bin\yubico-piv-tool -ArgumentList "-a change-puk -P RNADOMSI -N RNADOMSI" -Wait -NoNewWindow

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-a reset" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error blocking pin"
    }
}

function Yubico-SetManagementKey
{
    param (
        [string] $NewManagementKey = "010203040506070801020304050607080102030405060708"
    )

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-a set-mgm-key -n $NewManagementKey" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error setting management key"
    }
}

function Yubico-SetPin
{
    param (
        [string] $ManagementKey = "010203040506070801020304050607080102030405060708",
        [string] $OldPin = "123456",
        [string] $NewPin = ""
    )

    if ($pin.Length -gt 8)
    {
        throw "PIN is too long. Max 8 characters"
    }

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-k $ManagementKey -a change-pin -P $OldPin -N $NewPin" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error setting PIN"
    }
}

function Yubico-SetPuk
{
    param (
        [string] $ManagementKey = "010203040506070801020304050607080102030405060708",
        [string] $OldPuk = "12345678",
        [string] $NewPuk = ""
    )

    if ($pin.Length -gt 8)
    {
        throw "PUK is too long. Max 8 characters"
    }

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-k $ManagementKey -a change-puk -P $OldPuk -N $NewPuk" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error setting PUK"
    }
}

function Yubico-SetTriesCount
{
    param (
        [string] $ManagementKey = "010203040506070801020304050607080102030405060708",
        [int] $PinTries = 3,
        [int] $PukTries = 3
    )

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-v -a pin-retries --pin-retries $PinTries --puk-retries $PukTries" -Wait -NoNewWindow -PassThru -RedirectStandardError error -RedirectStandardOutput output

    if ($p.ExitCode -ne 0)
    {
        $p.ExitCode
	    throw "Error setting PIN/PUK tries, error code ${$p.ExitCode}"
    }
}

function Yubico-SetCHUID
{
    param (
        [string] $ManagementKey = "010203040506070801020304050607080102030405060708"
    )

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-k $ManagementKey -a set-chuid" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error setting CHUID"
    }
}

function Yubico-GenerateKey
{
    param (
        [string] $ManagementKey = "010203040506070801020304050607080102030405060708",
        [string] $OutputFile = "public.pem"
    )

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-k $ManagementKey -s 9a -a generate -o $OutputFile" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error generating key"
    }
}

function Yubico-GenerateCSR
{
    param (
        [string] $Pin = "123456",
        [string] $PublicKey= "123456",
        [string] $RequestFile = "request.csr"
    )

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList @"
-a verify-pin -P $Pin -s 9a -a request-certificate -S "/CN=example/O=test/" -i $PublicKey -o $RequestFile
"@ -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error generating CSR"
    }
}

function Yubico-Importcert
{
    param (
        [string] $ManagementKey = "010203040506070801020304050607080102030405060708",
        [string] $CertificateFile = "cert.crt"
    )

    $p = Start-Process .\bin\yubico-piv-tool -ArgumentList "-k $ManagementKey -s 9a -a import-certificate -i $CertificateFile" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error import cert"
    }
}

function Yubico-GetDeviceId
{
    $pinfo = New-Object System.Diagnostics.ProcessStartInfo
    $pinfo.FileName = "$pwd\bin\ykinfo"
    $pinfo.RedirectStandardOutput = $true
    $pinfo.UseShellExecute = $false
    $pinfo.Arguments = "-H"

    $p = New-Object System.Diagnostics.Process
    $p.StartInfo = $pinfo
    $p.Start() | Out-Null
    $p.WaitForExit()

    if ($p.ExitCode -ne 0)
    {
	    throw "Error getting serial"
    }

    $p.StandardOutput.ReadLine().Split(':')[1].Trim()
}

function Sign-OnBehalfOf
{
    param (
        [string] $EnrollmentAgentCert = "0102030405060708010203040506070801020304",
        [string] $User = "Domain\User",
        [string] $RequestFile = "request.csr",
        [string] $CertificateFile = "cert.crt"
    )

    $p = Start-Process .\bin\EOBOSigner -ArgumentList "$EnrollmentAgentCert $User $RequestFile $CertificateFile" -Wait -NoNewWindow -PassThru

    if ($p.ExitCode -ne 0)
    {
	    throw "Error signing cert"
    }
}

function Store-StringSecurely
{
    param (
        [string] $FileName,
        [string] $Text
    )

    # TODO: Encrypt file
    Set-Content -Path $FileName -Value $Text -Force
}

function Get-StringSecurely
{
    param (
        [string] $FileName
    )

    # TODO: Decrypt file
    Get-Content -Path $FileName
}

function Prompt-YesNo
{
    param (
        [String] $Title = "Delete Files",
        [String] $Message = "Do you want to delete the remaining files in the folder?",
        [String] $YesText = "Deletes all the files in the folder.",
        [String] $NoText = "Retains all the files in the folder."
    )

    $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", $YesText

    $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", $NoText

    $options = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no)

    $result = $host.ui.PromptForChoice($Title, $Message, $options, 0) 

    switch ($result)
    {
        0 { $true }
        1 { $false }
    }
}

function Remove-ItemIfExists
{
    param (
        [String] $path
    )

    if (Test-Path $path) 
    {
        Remove-Item $path
    }
}