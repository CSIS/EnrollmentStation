using System;

namespace YubicoLib.YubikeyPiv
{
    internal class YubikeyPivDeviceHandle : IDisposable
    {
        public IntPtr State { get; private set; }

        public YubikeyPivDeviceHandle()
        {
            IntPtr dev = IntPtr.Zero;
            YubikeyPivNative.YkPivInit(ref dev, 0);
            State = dev;
        }

        public void Dispose()
        {
            if (State != IntPtr.Zero)
                YubikeyPivNative.YkPivDone(State);

            State = IntPtr.Zero;
        }
    }
}