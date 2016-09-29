namespace EnrollmentStation.Code
{
    public static class YubikeyPolicyUtility
    {
        public static bool IsValidPin(string pin)
        {
            return pin?.Length >= 6;
        }
    }
}