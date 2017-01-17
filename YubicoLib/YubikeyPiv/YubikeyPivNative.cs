using System;
using System.Runtime.InteropServices;
using System.Text;

namespace YubicoLib.YubikeyPiv
{
    internal static class YubikeyPivNative
    {
        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_init", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivInit(ref IntPtr state, int verbose);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_done", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivDone(IntPtr state);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_connect", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivConnect(IntPtr state, string name);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_list_readers", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivListReaders(IntPtr state, IntPtr data, ref int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_disconnect", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivDisconnect(IntPtr state);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_transfer_data", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivTransferData(IntPtr state, byte[] templ, byte[] inData, int inLength, byte[] outData, ref int outLength, ref int sw);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_authenticate", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivAuthenticate(IntPtr state, byte[] key);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_set_mgmkey", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivSetManagementKey(IntPtr state, byte[] newKey);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_verify", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivVerify(IntPtr state, string pin, ref int tries);



        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_get_version", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivGetVersion(IntPtr state, StringBuilder version, int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_fetch_object", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivFetchObject(IntPtr state, int objectId, byte[] data, ref int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_save_object", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivSaveObject(IntPtr state, int objectId, byte[] data, int length);

        [DllImport("Binaries\\libykpiv-1.dll", EntryPoint = "ykpiv_sign_data", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoPivReturnCode YkPivSignData(IntPtr state, byte[] inData, int inLength, byte[] outData, ref int outLength, byte algorithm, byte key);

        public const int YKPIV_ALGO_3DES = 0x03;
        public const int YKPIV_ALGO_RSA1024 = 0x06;
        public const int YKPIV_ALGO_RSA2048 = 0x07;
        public const int YKPIV_ALGO_ECCP256 = 0x11;
        public const int YKPIV_ALGO_ECCP384 = 0x14;

        public const int YKPIV_KEY_AUTHENTICATION = 0x9a;
        public const int YKPIV_KEY_CARDMGM = 0x9b;
        public const int YKPIV_KEY_SIGNATURE = 0x9c;
        public const int YKPIV_KEY_KEYMGM = 0x9d;
        public const int YKPIV_KEY_CARDAUTH = 0x9e;

        public const int YKPIV_OBJ_CAPABILITY = 0x5fc107;
        public const int YKPIV_OBJ_CHUID = 0x5fc102;
        public const int YKPIV_OBJ_AUTHENTICATION = 0x5fc105;/* cert for 9a key */
        public const int YKPIV_OBJ_FINGERPRINTS = 0x5fc103;
        public const int YKPIV_OBJ_SECURITY = 0x5fc106;
        public const int YKPIV_OBJ_FACIAL = 0x5fc108;
        public const int YKPIV_OBJ_PRINTED = 0x5fc109;
        public const int YKPIV_OBJ_SIGNATURE = 0x5fc10a; /* cert for 9c key */
        public const int YKPIV_OBJ_KEY_MANAGEMENT = 0x5fc10b; /* cert for 9d key */
        public const int YKPIV_OBJ_CARD_AUTH = 0x5fc101;/* cert for 9e key */
        public const int YKPIV_OBJ_DISCOVERY = 0x7e;
        public const int YKPIV_OBJ_KEY_HISTORY = 0x5fc10c;
        public const int YKPIV_OBJ_IRIS = 0x5fc121;

        public const int YKPIV_INS_VERIFY = 0x20;
        public const int YKPIV_INS_CHANGE_REFERENCE = 0x24;
        public const int YKPIV_INS_RESET_RETRY = 0x2c;
        public const int YKPIV_INS_GENERATE_ASYMMETRIC = 0x47;
        public const int YKPIV_INS_AUTHENTICATE = 0x87;
        public const int YKPIV_INS_GET_DATA = 0xcb;
        public const int YKPIV_INS_PUT_DATA = 0xdb;

        /* Yubico vendor specific instructions */
        public const int YKPIV_INS_SET_MGMKEY = 0xff;
        public const int YKPIV_INS_IMPORT_KEY = 0xfe;
        public const int YKPIV_INS_GET_VERSION = 0xfd;
        public const int YKPIV_INS_RESET = 0xfb;
        public const int YKPIV_INS_SET_PIN_RETRIES = 0xfa;
    }
}