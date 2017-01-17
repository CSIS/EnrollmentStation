using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using EnrollmentStation.Code.Enums;

namespace EnrollmentStation.Api.YubikeyNeo
{
    public class YubikeyNeoManager : IDisposable
    {
        public static YubikeyNeoManager Instance { get; } = new YubikeyNeoManager();

        private YubikeyNeoManager()
        {
            YubicoNeoReturnCode code = YubikeyNeoNative.YkNeoManagerGlobalInit(1);

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

        public IEnumerable<string> ListDevices()
        {
            List<string> devices = new List<string>();

            using (YubikeyNeoDeviceHandle deviceHandle = new YubikeyNeoDeviceHandle())
            {
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    int len = 65 * 1024;
                    ptr = Marshal.AllocHGlobal(len);

                    YubicoNeoReturnCode res = YubikeyNeoNative.YkNeoManagerListDevices(deviceHandle.Device, ptr, ref len);
                    if (res != YubicoNeoReturnCode.YKNEOMGR_OK)
                        return devices;

                    byte[] data = new byte[len];
                    Marshal.Copy(ptr, data, 0, len);

                    int prev = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] != 0)
                            continue;

                        string strName = Encoding.ASCII.GetString(data, prev, i - prev);

                        if (!string.IsNullOrEmpty(strName))
                            devices.Add(strName);

                        prev = i + 1;
                    }
                }
                finally
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptr);
                }
            }

            return devices;
        }
    }
}