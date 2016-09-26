using System;

namespace EnrollmentStation.Code
{
    public static class YubikeyPolicyUtility
    {
        public static bool IsValidPin(Version yubikeyFirmware, string pin)
        {
            if (yubikeyFirmware >= new Version(4, 3, 1))
                return pin?.Length >= 6;

            return pin?.Length >= 1;
        }
    }
}