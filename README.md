# Enrollment Station using YubiKey Smart Cards

A Smart Card enrollment station for use in enterprises with Microsoft Active Directory Certificate Services and certificate-based logons. This software package will streamline some operations such as enrolling and terminating Smart Card devices.

This project is an entry in the YubiKing competition and should be considered a work in progress.

### Dependencies
* Windows PKI (Active Directory Certificate Services)
* Enrollment Agent Certificate (see prerequisites)
* CCID smart cards from Yubico (currently Premium NEO and Premium NEO-N)
* (optional) YubiHSM

## Prerequisites
To use this tool you will need an Enrollment Agent Certificate which allows you to enroll certificates on behalf of other users. This certificate template is available on a default Active Directory Certificate Services (Windows CA) installation, but is normally not permitted for any users other than Domain Admins. For added security, create a dedicated user which is used as the enrollment station user.

**A. Setup Enrollment Agent certificate template permissions**
1. Log on to your Windows Certificate Authority
2. Open the Certificate Authority control panel
3. Right click the 'Certificate Templates' folder and chose 'Manage'
4. Find the 'Enrollment Agent' template, right click on it and chose 'Properties'
5. In the security tab, allow your enrollment station user to enroll the certificate.

After a while, the template should be available to the enrollment station user through the Certificates snap-in in MMC.

**B. Enroll the 'Enrollment Agent' certificate**
1. Log on to your enrollment station as your enrollment user and open MMC.
2. In MMC, add the Certificates snap-in for the current user, and expand the Personal folder.
3. Right click on the Certificates folder and chose 'Request new Certificate'. Follow the wizard, and select the 'Enrollment Agent' template when asked for template.

After setting up the Enrollment Agent certificate, you need a YubiKey set to CCID Smart Card mode.

**C. Changing YubiKey to CCID Smart Card mode**
1. Download YubiKey NEO Manager from here https://developers.yubico.com/yubikey-neo-manager/Releases/
2. Under 'Devices' select 'YubiKey NEO'
3. Click 'Change connection mode'
4. Tick 'CCID* and click 'OK'

### First-run
Run 'Discover-EnrollmentAgent.ps1' to locate the thumbprint of the local Enrollment Agent certificate. The script will store the results in enrollmentagent.txt which will be read by the other scripts.

Run 'Generate-ManagementKey.ps1' to generate a 24 byte management key. The results are stored in 'ManagementKey.bin' for use on all future YubiKeys. For added security, make sure nobody but the enrollment station user can read the file.

## Operations

### Enrolling a user 
Run 'Enroll-NewYubikey.ps1' and follow the wizard. You will need to specify which user to enroll on behalf of, as well as set up a secure PIN and PUK code for the YubiKey.

### Termination of a Yubikey
To terminate a YubiKey, insert it into the enrollment station and run the 'Terminate-Yubikey.ps1'. It will do the following: 
* Reset the Yubikey, thus removing the data stored on it
* Revoke the certificate enrolled on the Yubikey (using data from the log created during enrollment)

If the YubiKey is lost or stolen, run 'Lost-Yubikey.ps1' instead. 

### Powershell CMDlet
The 'SharedFeatures.ps1' file contains common functionality used by the other scripts. These are:

* 'Generate-RandomString' generates a printable string - optionally using YubiHSM
* 'Generate-RandomStringHex' generates a HEX string - optionally using YubiHSM
* 'Yubico-ResetDevice'
* 'Yubico-SetManagementKey'
* 'Yubico-SetPin'
* 'Yubico-SetPuk'
* 'Yubico-SetTriesCount'
* 'Yubico-SetCHUID'
* 'Yubico-GenerateKey'
* 'Yubico-GenerateCSR'
* 'Yubico-Importcert'
* 'Yubico-GetDeviceId'
* 'Sign-OnBehalfOf' signs a certificate request on behalf of another user using the specified Enrollment Agent certificate
* 'Revoke-Certificate' revokes a specific certificate

Import the functionality by opening PowerShell in the folder with the 'SharedFeatures.ps1' file and write '. .\SharedFeatures.ps1'
Now you can call 'Generate-RandomString' and it will output a 12 characters random string.

## Todo
* Improve YubiHSM handling (remember choices, fail if missing and previously used)
* Handle more diverse PKI setups (possibly store a favourite CA in an options)
* Add more interactive texts to advise the user when its safe to remove a Yubikey
* Store some strings encrypted (e.g. Management Key)
* Verify data from AD (e.g. specified usernames to enroll on behalf of)