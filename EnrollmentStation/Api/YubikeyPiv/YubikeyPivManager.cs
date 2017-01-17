using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EnrollmentStation.Api.Utilities;
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
            byte[] data;
            using (YubikeyPivDeviceHandle deviceHandle = new YubikeyPivDeviceHandle())
            {
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    int len = 2048; // A typical reader name is 32 chars long. This gives space for 64 readers.
                    ptr = Marshal.AllocHGlobal(len);

                    IntPtr dev = deviceHandle.State;
                    YubicoPivReturnCode res = YubikeyPivNative.YkPivListReaders(dev, ptr, ref len);
                    if (res != YubicoPivReturnCode.YKPIV_OK)
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

            return StringUtils.ParseStrings(data);
        }

        public YubikeyPivDevice OpenDevice(string name)
        {
            return new YubikeyPivDevice(name);
        }
    }
}