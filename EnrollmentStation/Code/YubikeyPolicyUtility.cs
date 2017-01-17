using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnrollmentStation.Code
{
    public static class YubikeyPolicyUtility
    {
        private static readonly ReadOnlyCollection<YubikeyAlgorithm> Algorithms;

        static YubikeyPolicyUtility()
        {
            Algorithms = new List<YubikeyAlgorithm>
            {
                new YubikeyAlgorithm(YubikeyPivTool.YKPIV_ALGO_RSA1024, "RSA 1024"),
                new YubikeyAlgorithm(YubikeyPivTool.YKPIV_ALGO_RSA2048, "RSA 2048"),
                //new YubikeyAlgorithm(YubikeyPivTool.YKPIV_ALGO_RSA2048+1, "RSA 4096"),
                //new YubikeyAlgorithm(YubikeyPivTool.YKPIV_ALGO_ECCP256, "ECC P-256"),
                //new YubikeyAlgorithm(YubikeyPivTool.YKPIV_ALGO_ECCP384, "ECC P-384")
            }.AsReadOnly();
        }

        public static bool IsValidPin(string pin)
        {
            return pin?.Length >= 6;
        }

        public static ICollection<YubikeyAlgorithm> GetYubicoAlgorithms()
        {
            return Algorithms;
        }
    }
}