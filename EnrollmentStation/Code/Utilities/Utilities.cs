using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace EnrollmentStation.Code.Utilities
{
    public static class Utilities
    {
        public static byte[] GenerateRandomKey()
        {
            byte[] key1 = new byte[64];
            byte[] key2 = new byte[64];

            if (HsmRng.IsHsmPresent())
            {
                key1 = HsmRng.FetchRandom(key1.Length);

                using (RNGCryptoServiceProvider cryptoService = new RNGCryptoServiceProvider())
                    cryptoService.GetBytes(key2);
            }
            else
            {
                using (RNGCryptoServiceProvider cryptoService = new RNGCryptoServiceProvider())
                {
                    cryptoService.GetBytes(key1);
                    cryptoService.GetBytes(key2);
                }
            }

            byte[] full = key1.Concat(key2).ToArray();

            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(full);
            }
        }

        public static string MapBytesToString(byte[] data, string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678")
        {
            string res = string.Empty;

            for (int i = 0; i < data.Length; i++)
                res += characters[data[i] % characters.Length];

            return res;
        }

        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < (hex.Length >> 1); ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static string ExportPublicKeyToPEMFormat(RSAParameters parms)
        {
            RsaKeyParameters publicParams = DotNetUtilities.GetRsaPublicKey(parms);
            SubjectPublicKeyInfo keyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicParams);

            return Convert.ToBase64String(keyInfo.GetEncoded());
        }

        public static void InvokeIfNeeded(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}
