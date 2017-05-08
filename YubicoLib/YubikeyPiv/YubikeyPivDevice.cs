using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace YubicoLib.YubikeyPiv
{
    public class YubikeyPivDevice : IDisposable
    {
        private readonly YubikeyPivDeviceHandle _deviceHandle;

        public static byte[] DefaultManagementKey = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        public static string DefaultPin = "123456";
        public static string DefaultPuk = "12345678";

        internal YubikeyPivDevice(string name)
        {
            _deviceHandle = new YubikeyPivDeviceHandle();

            YubicoPivReturnCode code = YubikeyPivNative.YkPivConnect(_deviceHandle.State, name);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                throw new Exception("Unable to connect to PIV: " + code);
        }

        public void Dispose()
        {
            YubikeyPivNative.YkPivDisconnect(_deviceHandle.State);
            _deviceHandle.Dispose();
        }

        public Version GetVersion()
        {
            const int length = 256;

            StringBuilder sb = new StringBuilder(length);
            YubicoPivReturnCode code = YubikeyPivNative.YkPivGetVersion(_deviceHandle.State, sb, length);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                throw new Exception("Unable to fetch PIV version: " + code);

            return Version.Parse(sb.ToString());
        }

        public bool Authenticate(byte[] managementKey)
        {
            if (managementKey == null || managementKey.Length != 24)
                throw new ArgumentException("Must be 24 bytes");

            return YubikeyPivNative.YkPivAuthenticate(_deviceHandle.State, managementKey) == YubicoPivReturnCode.YKPIV_OK;
        }

        public int GetPinTriesLeft()
        {
            int triesLeft = -1;
            YubikeyPivNative.YkPivVerify(_deviceHandle.State, null, ref triesLeft);

            return triesLeft;
        }

        public bool VerifyPin(string pin, out int remainingTries)
        {
            int triesLeft = -1;
            YubicoPivReturnCode code = YubikeyPivNative.YkPivVerify(_deviceHandle.State, pin, ref triesLeft);

            remainingTries = triesLeft;

            if (code == YubicoPivReturnCode.YKPIV_OK)
                return true;

            return false;
        }

        public bool ChangePinPukRetries(byte pinRetryCount, byte pukRetryCount)
        {
            if (pinRetryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(pinRetryCount));
            if (pukRetryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(pukRetryCount));

            byte[] templ = { 0, YubikeyPivNative.YKPIV_INS_SET_PIN_RETRIES, pinRetryCount, pukRetryCount };
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            YubicoPivReturnCode code = YubikeyPivNative.YkPivTransferData(_deviceHandle.State, templ, null, 0, outData, ref outLength, ref sw);

            return code == YubicoPivReturnCode.YKPIV_OK && sw == YubikeyPivNative.SW_SUCCESS;
        }

        public bool ChangePin(string oldPin, string pin, out int remainingTries)
        {
            byte[] templ = { 0, YubikeyPivNative.YKPIV_INS_CHANGE_REFERENCE, 0, 0x80 };
            byte[] inData = new byte[16];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            for (int i = 0; i < inData.Length; i++)
                inData[i] = 0xFF;

            // Set up PUK and NEWPUK
            Encoding.ASCII.GetBytes(oldPin, 0, Math.Min(8, oldPin.Length), inData, 0);
            Encoding.ASCII.GetBytes(pin, 0, Math.Min(8, pin.Length), inData, 8);

            YubicoPivReturnCode code = YubikeyPivNative.YkPivTransferData(_deviceHandle.State, templ, inData, inData.Length, outData, ref outLength, ref sw);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                remainingTries = -1;
                return false;
            }

            if (sw != YubikeyPivNative.SW_SUCCESS)
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
            byte[] templ = { 0, YubikeyPivNative.YKPIV_INS_CHANGE_REFERENCE, 0, 0x81 };
            byte[] inData = new byte[16];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            for (int i = 0; i < inData.Length; i++)
                inData[i] = 0xFF;

            // Set up PUK and NEWPUK
            Encoding.ASCII.GetBytes(oldPuk, 0, Math.Min(8, oldPuk.Length), inData, 0);
            Encoding.ASCII.GetBytes(puk, 0, Math.Min(8, puk.Length), inData, 8);

            YubicoPivReturnCode code = YubikeyPivNative.YkPivTransferData(_deviceHandle.State, templ, inData, inData.Length, outData, ref outLength, ref sw);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                remainingTries = -1;
                return false;
            }

            if (sw != YubikeyPivNative.SW_SUCCESS)
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

        public bool GenerateKey9a(byte algorithm, out RSAParameters publicKey)
        {
            publicKey = new RSAParameters();

            byte[] templ = { 0, YubikeyPivNative.YKPIV_INS_GENERATE_ASYMMETRIC, 0, 0x9A };
            byte[] inData = new byte[5];    // TODO: Newer versions of yubico-piv-tool use 11 bytes of data, see: https://github.com/Yubico/yubico-piv-tool/blob/b08de955970c5cd544c740990fb68f496fedb814/tool/yubico-piv-tool.c#L122
            byte[] outData = new byte[1024];
            int outLength = outData.Length, sw = -1;

            // Set up IN
            inData[0] = 0xAC;
            inData[1] = 3;
            inData[2] = 0x80;
            inData[3] = 1;
            inData[4] = algorithm;

            YubicoPivReturnCode code = YubikeyPivNative.YkPivTransferData(_deviceHandle.State, templ, inData, inData.Length, outData, ref outLength, ref sw);

            if (code != YubicoPivReturnCode.YKPIV_OK)
            {
                return false;
            }

            if (sw != YubikeyPivNative.SW_SUCCESS)
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

        //    YubicoPivReturnCode code = YkPivSignData(_deviceHandle.State, toSign, toSign.Length, result, ref outputLength, YKPIV_ALGO_RSA2048, key);

        //    if (code != YubicoPivReturnCode.YKPIV_OK)
        //        return false;


        //    return true;
        //}

        public bool SetManagementKey(byte[] newKey)
        {
            if (newKey == null || newKey.Length != 24)
                throw new ArgumentException("Must be 24 bytes");

            return YubikeyPivNative.YkPivSetManagementKey(_deviceHandle.State, newKey) == YubicoPivReturnCode.YKPIV_OK;
        }

        public bool ResetDevice()
        {
            byte[] templ = { 0, YubikeyPivNative.YKPIV_INS_RESET, 0, 0 };
            byte[] inData = new byte[0];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            YubicoPivReturnCode code = YubikeyPivNative.YkPivTransferData(_deviceHandle.State, templ, inData, inData.Length, outData, ref outLength, ref sw);

            return code == YubicoPivReturnCode.YKPIV_OK && sw == YubikeyPivNative.SW_SUCCESS;
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
                code = YubikeyPivNative.YkPivFetchObject(_deviceHandle.State, YubikeyPivNative.YKPIV_OBJ_AUTHENTICATION, data, ref tmpLength);

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

            YubicoPivReturnCode code = YubikeyPivNative.YkPivSaveObject(_deviceHandle.State, YubikeyPivNative.YKPIV_OBJ_AUTHENTICATION, data, offset);

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

            YubicoPivReturnCode code = YubikeyPivNative.YkPivSaveObject(_deviceHandle.State, YubikeyPivNative.YKPIV_OBJ_CHUID, newChuid, newChuid.Length);

            if (code != YubicoPivReturnCode.YKPIV_OK)
                return false;

            return true;
        }

        public bool GetCHUID(out byte[] chuid)
        {
            byte[] tmp = new byte[2048];
            int length = tmp.Length;

            YubicoPivReturnCode code = YubikeyPivNative.YkPivFetchObject(_deviceHandle.State, YubikeyPivNative.YKPIV_OBJ_CHUID, tmp, ref length);

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
            byte[] templ = { 0, YubikeyPivNative.YKPIV_INS_RESET_RETRY, 0, 0x80 };
            byte[] inData = new byte[16];
            byte[] outData = new byte[256];
            int outLength = outData.Length, sw = -1;

            for (int i = 0; i < inData.Length; i++)
                inData[i] = 0xFF;

            // Set up PUK and NEWPUK
            Encoding.ASCII.GetBytes(puk, 0, Math.Min(8, puk.Length), inData, 0);
            Encoding.ASCII.GetBytes(newPin, 0, Math.Min(8, newPin.Length), inData, 8);

            YubicoPivReturnCode code = YubikeyPivNative.YkPivTransferData(_deviceHandle.State, templ, inData, inData.Length, outData, ref outLength, ref sw);

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
    }
}