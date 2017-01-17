using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using EnrollmentStation.Api.YubikeyNeo;
using EnrollmentStation.Code.Enums;

namespace EnrollmentStation.Api.YubikeyPiv
{
    public class YubikeyPivManager
    {
        public static YubikeyPivManager Instance { get; } = new YubikeyPivManager();

        private YubikeyPivManager()
        {

        }

        public IEnumerable<string> ListDevices()
        {
            List<string> devices = new List<string>();

            using (YubikeyPivDeviceHandle deviceHandle = new YubikeyPivDeviceHandle())
            {
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    int len = 65 * 1024;
                    ptr = Marshal.AllocHGlobal(len);

                    IntPtr dev = deviceHandle.State;
                    YubicoPivReturnCode res = YubikeyPivNative.YkPivListReaders(ref dev, ptr, ref len);
                    if (res != YubicoPivReturnCode.YKPIV_OK)
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

        public YubikeyPivDevice OpenDevice(string name)
        {
            return new YubikeyPivDevice(name);
        }
    }
}