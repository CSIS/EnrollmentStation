using System;

namespace EnrollmentStation.Api.YubikeyNeo
{
    internal class YubikeyNeoDeviceHandle : IDisposable
    {
        public IntPtr Device { get; private set; }

        public YubikeyNeoDeviceHandle()
        {
            IntPtr intPtr = IntPtr.Zero;
            YubikeyNeoNative.YkNeoManagerInit(ref intPtr);
            Device = intPtr;
        }

        public void Dispose()
        {
            if (Device != IntPtr.Zero)
                YubikeyNeoNative.YkNeoManagerDone(Device);

            Device = IntPtr.Zero;
        }
    }
}