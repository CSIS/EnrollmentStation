# CSIS Enrollment Station using Yubico Smartcard

A SmartCard enrollment station for use in enterprises utilising Microsoft Active Directory Certificate Services and certificate-based logons. This software package will streamline some operations such as enrolling and terminating Smart Card devices.

This is a work in progress.

### Dependencies

* Item 1
* Item 2

## Todo

* Improve YubiHSM handling (remember choices, fail if missing and previously used)
* Add more interactive texts to advise the user when its safe to remove a Yubikey
* Handle more diverse PKI setups (possibly store a favourite CA in an options)
* Store some strings encrypted (e.g. Management Key)
* Verify data from AD (e.g. specified usernames to enroll on behalf of)

## Operations

There are basically two operations (enroll and terminate/lost), as well as a few first-time setup operations.

### First-run

`Discover-EnrollmentAgent.ps1` is run to locate the Thumbprint of the local Enrollment Agent certificate. This script is interactive and will store its results to `enrollmentagent.txt`.

`Generate-ManagementKey.ps1` is run to generate a 24-byte management key in `ManagementKey.bin` for use on all future Yubikeys. This should be stored securely.

### Enrolling 

`Enroll-NewYubikey.ps1` is an interactive guide to help specify which user to enroll for, as well as set up a secure `PIN`, `PUK` and `Management Key` for the inserted Yubikey.

### Termination and Lost Yubikeys

`Terminate-Yubikey.ps1` is used to Terminate an inserted Yubikey. This will 
* Reset the Yubikey, thus removing the data stored on it
* Revoke the certificate enrolled on the Yubikey (using data from the `log`)

`Lost-Yubikey.ps1` is used to revoke a single certificate to which you do not have the Yubikey (hence a lost Yubikey). It will use only the information in the `log` to find the relevant certificate certificate. 

### Shared features

There are a series of cmdlets in the `SharedFeatures.ps1` file. These are:

* `Generate-RandomString` generates a printable string - possibly using YubiHSM
* `Generate-RandomStringHex` generates a HEX string - possibly using YubiHSM
* `Yubico-ResetDevice`
* `Yubico-SetManagementKey`
* `Yubico-SetPin`
* `Yubico-SetPuk`
* `Yubico-SetTriesCount`
* `Yubico-SetCHUID`
* `Yubico-GenerateKey`
* `Yubico-GenerateCSR`
* `Yubico-Importcert`
* `Yubico-GetDeviceId`
* `Sign-OnBehalfOf` signs a certificate request on behalf of another user using the specified Enrollment Agent certificate
* `Revoke-Certificate` revokes a specific certificate