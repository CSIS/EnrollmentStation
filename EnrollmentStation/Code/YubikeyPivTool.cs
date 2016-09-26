using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EnrollmentStation.Code
{
    public class YubikeyPivTool : IDisposable
    {
        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_init", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivInit(ref IntPtr state, int verbose);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_done", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivDone(IntPtr state);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_connect", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivConnect(IntPtr state, string wanted);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_disconnect", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivDisconnect(IntPtr state);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_get_version", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivGetVersion(IntPtr state, StringBuilder version, int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_fetch_object", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivFetchObject(IntPtr state, int objectId, byte[] data, ref int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_save_object", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivSaveObject(IntPtr state, int objectId, byte[] data, int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_verify", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivVerifyPin(IntPtr state, string pin, ref int tries);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_authenticate", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivAuthenticate(IntPtr state, byte[] key);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_set_mgmkey", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivSetManagementKey(IntPtr state, byte[] newKey);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_sign_data", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivSignData(IntPtr state, byte[] inData, int inLength, byte[] outData, ref int outLength, byte algorithm, byte key);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_transfer_data", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoPivReturnCode YkPivTransferData(IntPtr state, byte[] templ, byte[] inData, int inLength, byte[] outData, ref int outLength, ref int sw);

        private const int YKPIV_ALGO_3DES = 0x03;
        private const int YKPIV_ALGO_RSA1024 = 0x06;
        private const int YKPIV_ALGO_RSA2048 = 0x07;
        private const int YKPIV_ALGO_ECCP256 = 0x11;

        private const int YKPIV_KEY_AUTHENTICATION = 0x9a;
        private const int YKPIV_KEY_CARDMGM = 0x9b;
        private const int YKPIV_KEY_SIGNATURE = 0x9c;
        private const int YKPIV_KEY_KEYMGM = 0x9d;
        private const int YKPIV_KEY_CARDAUTH = 0x9e;

        private const int YKPIV_OBJ_CAPABILITY = 0x5fc107;
        private const int YKPIV_OBJ_CHUID = 0x5fc102;
        private const int YKPIV_OBJ_AUTHENTICATION = 0x5fc105;/* cert for 9a key */
        private const int YKPIV_OBJ_FINGERPRINTS = 0x5fc103;
        private const int YKPIV_OBJ_SECURITY = 0x5fc106;
        private const int YKPIV_OBJ_FACIAL = 0x5fc108;
        private const int YKPIV_OBJ_PRINTED = 0x5fc109;
        private const int YKPIV_OBJ_SIGNATURE = 0x5fc10a; /* cert for 9c key */
        private const int YKPIV_OBJ_KEY_MANAGEMENT = 0x5fc10b; /* cert for 9d key */
        private const int YKPIV_OBJ_CARD_AUTH = 0x5fc101;/* cert for 9e key */
        private const int YKPIV_OBJ_DISCOVERY = 0x7e;
        private const int YKPIV_OBJ_KEY_HISTORY = 0x5fc10c;
        private const int YKPIV_OBJ_IRIS = 0x5fc121;

        private const int YKPIV_INS_VERIFY = 0x20;
        private const int YKPIV_INS_CHANGE_REFERENCE = 0x24;
        private const int YKPIV_INS_RESET_RETRY = 0x2c;
        private const int YKPIV_INS_GENERATE_ASYMMETRIC = 0x47;
        private const int YKPIV_INS_AUTHENTICATE = 0x87;
        private const int YKPIV_INS_GET_DATA = 0xcb;
        private const int YKPIV_INS_PUT_DATA = 0xdb;

        /* Yubico vendor specific instructions */
        private const int YKPIV_INS_SET_MGMKEY = 0xff;
        private const int YKPIV_INS_IMPORT_KEY = 0xfe;
        private const int YKPIV_INS_GET_VERSION = 0xfd;
        private const int YKPIV_INS_RESET = 0xfb;
        private const int YKPIV_INS_SET_PIN_RETRIES = 0xfa;

        private IntPtr _state = IntPtr.Zero;

        public static byte[] DefaultManagementKey = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        public static string DefaultPin = "123456";
        public static string DefaultPuk = "12345678";

        public static YubikeyPivTool StartPiv()
        {
            return new YubikeyPivTool();
        }

        internal YubikeyPivTool()
        {
            YubicoPivReturnCode code = YkPivInit(ref _state, 1);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                throw new Exception("Unable to init PIV: " + code);

            code = YkPivConnect(_state, null);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                throw new Exception("Unable to connect to PIV: " + code);
        }

        public string GetVersion()
        {
            const int length = 256;

            StringBuilder sb = new StringBuilder(length);
            YubicoPivReturnCode code = YkPivGetVersion(_state, sb, length);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                throw new Exception("Unable to fetch PIV version: " + code);

            return sb.ToString();
        }

        public bool Authenticate(byte[] managementKey)
        {
            if (managementKey == null || managementKey.Length != 24)
                throw new ArgumentException("Must be 24 bytes");

            return YkPivAuthenticate(_state, managementKey) == YubicoPivReturnCode.YKPIV_OK;
        }

        public int GetPinTriesLeft()
        {
            int triesLeft = -1;
            YkPivVerifyPin(_state, null, ref triesLeft);

            return triesLeft;
        }

        public bool VerifyPin(string pin, out int remainingTries)
        {
            int triesLeft = -1;
            YubicoPivReturnCode code = YkPivVerifyPin(_state, pin, ref triesLeft);

            remainingTries = triesLeft;

            if (code == YubicoPivReturnCode.YKPIV_OK)
                return true;

            return false;
        }

        public bool ChangePin(string oldPin, string pin, out int remainingTries)
        {
            byte[] templ = { 0, YKPIV_INS_CHANGE_REFERENCE, 0, 0x80 };
            byte[] inData = new byte[16];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            for (int i = 0; i < inData.Length; i++)
                inData[i] = 0xFF;

            // Set up PUK and NEWPUK
            Encoding.ASCII.GetBytes(oldPin, 0, Math.Min(8, oldPin.Length), inData, 0);
            Encoding.ASCII.GetBytes(pin, 0, Math.Min(8, pin.Length), inData, 8);

            YubicoPivReturnCode code = YkPivTransferData(_state, templ, inData, inData.Length, outData, ref outLength, ref sw);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                remainingTries = -1;
                return false;
            }

            if (sw != 0x9000)
            {
                if ((sw >> 8) == 0x63)
                {
                    remainingTries = sw & 0xff;

                    return false;
                }

                if (sw == 0x6983)
                {
                    remainingTries = 0;
                    return false;
                }
            }

            remainingTries = -1;
            return true;
        }

        public bool ChangePuk(string oldPuk, string puk, out int remainingTries)
        {
            byte[] templ = { 0, YKPIV_INS_CHANGE_REFERENCE, 0, 0x81 };
            byte[] inData = new byte[16];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            for (int i = 0; i < inData.Length; i++)
                inData[i] = 0xFF;

            // Set up PUK and NEWPUK
            Encoding.ASCII.GetBytes(oldPuk, 0, Math.Min(8, oldPuk.Length), inData, 0);
            Encoding.ASCII.GetBytes(puk, 0, Math.Min(8, puk.Length), inData, 8);

            YubicoPivReturnCode code = YkPivTransferData(_state, templ, inData, inData.Length, outData, ref outLength, ref sw);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                remainingTries = -1;
                return false;
            }

            if (sw != 0x9000)
            {
                if ((sw >> 8) == 0x63)
                {
                    remainingTries = sw & 0xff;

                    return false;
                }

                if (sw == 0x6983)
                {
                    remainingTries = 0;
                    return false;
                }
            }

            remainingTries = -1;
            return true;
        }

        public bool GenerateKey9a(out RSAParameters publicKey)
        {
            publicKey = new RSAParameters();

            byte[] templ = { 0, YKPIV_INS_GENERATE_ASYMMETRIC, 0, 0x9A };
            byte[] inData = new byte[5];    // TODO: Newer versions of yubico-piv-tool use 11 bytes of data, see: https://github.com/Yubico/yubico-piv-tool/blob/b08de955970c5cd544c740990fb68f496fedb814/tool/yubico-piv-tool.c#L122
            byte[] outData = new byte[1024];
            int outLength = outData.Length, sw = -1;

            // Set up IN
            inData[0] = 0xAC;
            inData[1] = 3;
            inData[2] = 0x80;
            inData[3] = 1;
            inData[4] = YKPIV_ALGO_RSA2048;     // Possible accept different algorithms in the future

            YubicoPivReturnCode code = YkPivTransferData(_state, templ, inData, inData.Length, outData, ref outLength, ref sw);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                return false;
            }

            if (sw != 0x9000)
            {
                return false;
            }

            // Skip first 2 bytes
            outData = outData.Skip(2).ToArray();

            int dataLength;
            int offset = GetDataOffsetAndLength(outData, out dataLength);

            outData = outData.Skip(offset).ToArray();

            if (outData[0] != 0x81)
                throw new InvalidOperationException("Received bad public key from yubikey");

            Array.Copy(outData, 1, outData, 0, outData.Length - 1);

            offset = GetDataOffsetAndLength(outData, out dataLength);

            byte[] modulus = outData.Skip(offset).Take(dataLength).ToArray();
            outData = outData.Skip(offset + dataLength).ToArray();

            if (outData[0] != 0x82)
                throw new InvalidOperationException("Received bad public key structure from yubikey");

            Array.Copy(outData, 1, outData, 0, outData.Length - 1);

            offset = GetDataOffsetAndLength(outData, out dataLength);
            byte[] exponent = outData.Skip(offset).Take(dataLength).ToArray();

            publicKey.Modulus = modulus;
            publicKey.Exponent = exponent;

            return true;
        }

        //public bool SignData(byte[] toSign, out byte[] signature)
        //{
        //    byte key = 0x9A; // Slot 9a

        //    signature = null;
        //    byte[] result = new byte[256];
        //    int outputLength = result.Length;

        //    YubicoPivReturnCode code = YkPivSignData(_state, toSign, toSign.Length, result, ref outputLength, YKPIV_ALGO_RSA2048, key);

        //    if (code != YubicoPivReturnCode.YKPIV_OK)
        //        return false;


        //    return true;
        //}

        public bool SetManagementKey(byte[] newKey)
        {
            if (newKey == null || newKey.Length != 24)
                throw new ArgumentException("Must be 24 bytes");

            return YkPivSetManagementKey(_state, newKey) == YubicoPivReturnCode.YKPIV_OK;
        }

        public bool ResetDevice()
        {
            byte[] templ = { 0, YKPIV_INS_RESET, 0, 0 };
            byte[] inData = new byte[0];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            YubicoPivReturnCode code = YkPivTransferData(_state, templ, inData, inData.Length, outData, ref outLength, ref sw);

            return code == YubicoPivReturnCode.YKPIV_OK && sw == 0x9000;
        }

        public X509Certificate2 GetCertificate9a()
        {
            byte[] data;
            int length = 2048;

            YubicoPivReturnCode code;

            do
            {
                length *= 2;

                data = new byte[length];
                int tmpLength = length;
                code = YkPivFetchObject(_state, YKPIV_OBJ_AUTHENTICATION, data, ref tmpLength);

                if (code == YubicoPivReturnCode.YKPIV_GENERIC_ERROR)
                    // Object is not set
                    return null;

                if (code == YubicoPivReturnCode.YKPIV_OK && code == YubicoPivReturnCode.YKPIV_SIZE_ERROR)
                    continue;

                // Shift up 1 byte to skip the first
                Array.Copy(data, 1, data, 0, data.Length - 1);

                // Resize for later
                Array.Resize(ref data, tmpLength - 1);
            } while (code == YubicoPivReturnCode.YKPIV_SIZE_ERROR);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                throw new Exception("Unable to fetch for PIV certificate 9a: " + code);

            int certLength;
            int offset = GetDataOffsetAndLength(data, out certLength);

            Array.Copy(data, offset, data, 0, certLength);
            //Array.Resize(ref data, certLength);

            return new X509Certificate2(data);
        }

        public YubicoPivReturnCode SetCertificate9a(X509Certificate2 cert)
        {
            byte[] certData = cert.GetRawCertData();
            byte[] data = new byte[certData.Length + 1 + 3 + 5];

            data[0] = 0x70;
            int offset = 1;
            offset += SetDataLength(data, offset, certData.Length);
            Array.Copy(certData, 0, data, offset, certData.Length);

            offset += certData.Length;

            data[offset++] = 0x71;
            data[offset++] = 1;
            data[offset++] = 0; // certinfo (gzip etc)
            data[offset++] = 0xFE; // LRC
            data[offset++] = 0;

            YubicoPivReturnCode code = YkPivSaveObject(_state, YKPIV_OBJ_AUTHENTICATION, data, offset);
            
            return code;
        }

        public bool SetCHUID(Guid newId, out byte[] newChuid)
        {
            newChuid = new byte[]
            {
                0x30, 0x19, 0xd4, 0xe7, 0x39, 0xda, 0x73, 0x9c, 0xed, 0x39, 0xce, 0x73, 0x9d,
                0x83, 0x68, 0x58, 0x21, 0x08, 0x42, 0x10, 0x84, 0x21, 0x38, 0x42, 0x10, 0xc3,
                0xf5, 0x34, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x35, 0x08, 0x32, 0x30, 0x33, 0x30, 0x30,
                0x31, 0x30, 0x31, 0x3e, 0x00, 0xfe, 0x00
            };

            int writeOffset = 29;

            byte[] guidBytes = newId.ToByteArray();
            Array.Copy(guidBytes, 0, newChuid, writeOffset, guidBytes.Length);

            YubicoPivReturnCode code = YkPivSaveObject(_state, YKPIV_OBJ_CHUID, newChuid, newChuid.Length);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                return false;

            return true;
        }

        public bool GetCHUID(out byte[] chuid)
        {
            byte[] tmp = new byte[2048];
            int length = tmp.Length;

            YubicoPivReturnCode code = YkPivFetchObject(_state, YKPIV_OBJ_CHUID, tmp, ref length);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                chuid = null;
                return false;
            }

            chuid = new byte[length];
            Array.Copy(tmp, chuid, length);

            return true;
        }

        private static int SetDataLength(byte[] buffer, int offset, int length)
        {
            if (length < 0x80)
            {
                buffer[offset] = (byte)length;
                return 1;
            }

            if (length < 0xFF)
            {
                buffer[offset] = 0x81;
                buffer[offset + 1] = (byte)length;
                return 2;
            }

            {
                buffer[offset] = 0x82;
                buffer[offset + 1] = (byte)((length >> 8) & 0xFF);
                buffer[offset + 2] = (byte)(length & 0xFF);
                return 3;
            }
        }

        private static int GetDataOffsetAndLength(byte[] data, out int dataLength)
        {
            if (data[0] < 0x81)
            {
                dataLength = data[0];
                return 1;
            }

            if ((data[0] & 0x7F) == 1)
            {
                dataLength = data[1];
                return 2;
            }

            if ((data[0] & 0x7F) == 2)
            {
                dataLength = (data[1] << 8) + data[2];
                return 3;
            }

            // Should not happen
            dataLength = data.Length;
            return 0;
        }

        public void BlockPin()
        {
            string randomPin = "PASSWORD";

            int tmpRemaining;
            do
            {
                bool success = VerifyPin(randomPin, out tmpRemaining);

                if (success)
                {
                    // Wow, someone had PASSWORD as their PIN. Alter our test.
                    randomPin = "DROWSSAP";
                }
            } while (tmpRemaining > 0);
        }

        public bool UnblockPin(string puk, string newPin)
        {
            byte[] templ = { 0, YKPIV_INS_RESET_RETRY, 0, 0x80 };
            byte[] inData = new byte[16];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            for (int i = 0; i < inData.Length; i++)
                inData[i] = 0xFF;

            // Set up PUK and NEWPUK
            Encoding.ASCII.GetBytes(puk, 0, Math.Min(8, puk.Length), inData, 0);
            Encoding.ASCII.GetBytes(newPin, 0, Math.Min(8, newPin.Length), inData, 8);

            YubicoPivReturnCode code = YkPivTransferData(_state, templ, inData, inData.Length, outData, ref outLength, ref sw);

            return code == YubicoPivReturnCode.YKPIV_OK;
        }

        public void BlockPuk()
        {
            string randomPin = "PASSWORD";

            int tmpRemaining;
            do
            {
                bool success = ChangePuk(randomPin, randomPin, out tmpRemaining);

                if (success)
                {
                    // Wow, someone had PASSWORD as their PUK. Alter our test.
                    randomPin = "DROWSSAP";
                }
            } while (tmpRemaining > 0);
        }

        public void Dispose()
        {
            YkPivDisconnect(_state);
            YkPivDone(_state);
        }
    }
}