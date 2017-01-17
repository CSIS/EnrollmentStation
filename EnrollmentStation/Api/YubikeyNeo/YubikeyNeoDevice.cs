using System;
using System.Runtime.InteropServices;
using System.Text;
using EnrollmentStation.Code;
using EnrollmentStation.Code.Enums;

namespace EnrollmentStation.Api.YubikeyNeo
{
    public class YubikeyNeoDevice : IDisposable
    {
        private IntPtr _currentDevice;

        internal YubikeyNeoDevice(string name)
        {
            YubicoNeoReturnCode code = YubikeyNeoNative.YkNeoManagerInit(ref _currentDevice);

            if (code != YubicoNeoReturnCode.YKNEOMGR_OK)
                throw new Exception("Unable to init: " + code);

            YubikeyNeoNative.YkNeoManagerConnect(_currentDevice, name);
        }

        public void Dispose()
        {
            if (_currentDevice == IntPtr.Zero)
                return;

            YubikeyNeoNative.YkNeoManagerDone(_currentDevice);
            _currentDevice = IntPtr.Zero;
        }

        public int GetSerialNumber()
        {
            if (_currentDevice == IntPtr.Zero)
                return -1;

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            return YubikeyNeoNative.YkNeoManagerGetSerialNumber(_currentDevice);
        }

        public Version GetVersion()
        {
            if (_currentDevice == IntPtr.Zero)
                return new Version();

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            byte major = YubikeyNeoNative.YkNeoManagerGetVersionMajor(_currentDevice);
            byte minor = YubikeyNeoNative.YkNeoManagerGetVersionMinor(_currentDevice);
            byte build = YubikeyNeoNative.YkNeoManagerGetVersionBuild(_currentDevice);

            return new Version(major, minor, build);
        }

        public YubicoNeoMode GetMode()
        {
            if (_currentDevice == IntPtr.Zero)
                return new YubicoNeoMode(0);

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            return new YubicoNeoMode(YubikeyNeoNative.YkNeoManagerGetMode(_currentDevice));
        }

        public void SetMode(YubicoNeoModeEnum mode)
        {
            if (_currentDevice == IntPtr.Zero)
                return;

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            YubicoNeoReturnCode code = YubikeyNeoNative.YkNeoManagerSetMode(_currentDevice, mode);

            if (code != YubicoNeoReturnCode.YKNEOMGR_OK)
                throw new Exception("Unable to set mode: " + code);
        }
    }
}