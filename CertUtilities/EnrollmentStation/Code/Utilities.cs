using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;

namespace EnrollmentStation.Code
{
    public static class Utilities
    {
        public static string MapBytesToString(byte[] data, string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678")
        {
            string res = "";

            for (int i = 0; i < data.Length; i++)
                res += characters[data[i] % characters.Length];

            return res;
        }

        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
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
            RsaKeyParameters publicParams = Org.BouncyCastle.Security.DotNetUtilities.GetRsaPublicKey(parms);
            SubjectPublicKeyInfo keyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicParams);

            return Convert.ToBase64String(keyInfo.GetEncoded());
        }
    }
}
