using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using YubicoLib.Utilities;

namespace YubicoLib.YubikeyNeo
{
    public class YubikeyNeoManager : IDisposable
    {
        public static YubikeyNeoManager Instance { get; } = new YubikeyNeoManager();

        private YubikeyNeoManager()
        {
            YubicoNeoReturnCode code = YubikeyNeoNative.YkNeoManagerGlobalInit(0);

            if (code != YubicoNeoReturnCode.YKNEOMGR_OK)
                throw new Exception("Unable to init global: " + code);
        }

        public void Dispose()
        {
            YubikeyNeoNative.YkNeoManagerGlobalDone();
        }

        public YubikeyNeoDevice OpenDevice(string name)
        {
            return new YubikeyNeoDevice(name);
        }

        public IEnumerable<string> ListDevices(bool filter = true)
        {
            byte[] data;
            using (YubikeyNeoDeviceHandle deviceHandle = new YubikeyNeoDeviceHandle())
            {
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    int len = 2048; // A typical reader name is 32 chars long. This gives space for 64 readers.
                    ptr = Marshal.AllocHGlobal(len);

                    YubicoNeoReturnCode res = YubikeyNeoNative.YkNeoManagerListDevices(deviceHandle.Device, ptr, ref len);
                    if (res != YubicoNeoReturnCode.YKNEOMGR_OK)
                        return Enumerable.Empty<string>();

                    data = new byte[len];
                    Marshal.Copy(ptr, data, 0, len);


                }
                finally
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptr);
                }
            }

            if (filter)
                return StringUtils.ParseStrings(data).Where(IsValidDevice);

            return StringUtils.ParseStrings(data);
        }

        public bool IsValidDevice(string name)
        {
            return name.StartsWith("Yubico") && (name.Contains("Yubikey NEO ") || name.Contains("Yubikey 4 "));
        }
    }
}