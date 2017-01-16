using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using EnrollmentStation.Code.Enums;

namespace EnrollmentStation.Code
{
    public class YubikeyNeoManager : IDisposable
    {
        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_global_init", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerGlobalInit(int initFlags);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_global_done", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void YkNeoManagerGlobalDone();

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_init", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerInit(ref IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_done", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void YkNeoManagerDone(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_mode", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoModeEnum YkNeoManagerGetMode(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_modeswitch", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerSetMode(IntPtr dev, YubicoNeoModeEnum mode);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_serialno", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int YkNeoManagerGetSerialNumber(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_discover", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerDiscover(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_discover_match", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerDiscoverMatch(IntPtr dev, IntPtr match);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_connect", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerConnect(IntPtr dev, IntPtr match);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_version_major", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern byte YkNeoManagerGetVersionMajor(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_version_minor", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern byte YkNeoManagerGetVersionMinor(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_version_build", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern byte YkNeoManagerGetVersionBuild(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_list_devices", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern YubicoNeoReturnCode YkNeoManagerListDevices(IntPtr dev, IntPtr buffer, ref int length);

        public static YubikeyNeoManager Instance { get; } = new YubikeyNeoManager();

        private IntPtr _currentDevice;

        private YubikeyNeoManager()
        {
            Init();
        }

        public void Init()
        {
            YubicoNeoReturnCode code = YkNeoManagerGlobalInit(1);

            if (code != YubicoNeoReturnCode.YKNEOMGR_OK)
                throw new Exception("Unable to init: " + code);

            YkNeoManagerInit(ref _currentDevice);
        }

        public void Close()
        {
            YkNeoManagerDone(_currentDevice);
            _currentDevice = IntPtr.Zero;

            YkNeoManagerGlobalDone();
        }

        public void Dispose()
        {
            if (_currentDevice == IntPtr.Zero)
                return;

            YkNeoManagerDone(_currentDevice);
            _currentDevice = IntPtr.Zero;

            YkNeoManagerGlobalDone();
        }

        public int GetSerialNumber()
        {
            if (_currentDevice == IntPtr.Zero)
                return -1;

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            return YkNeoManagerGetSerialNumber(_currentDevice);
        }

        public Version GetVersion()
        {
            if (_currentDevice == IntPtr.Zero)
                return new Version();

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            byte major = YkNeoManagerGetVersionMajor(_currentDevice);
            byte minor = YkNeoManagerGetVersionMinor(_currentDevice);
            byte build = YkNeoManagerGetVersionBuild(_currentDevice);

            return new Version(major, minor, build);
        }

        public YubicoNeoMode GetMode()
        {
            if (_currentDevice == IntPtr.Zero)
                return new YubicoNeoMode(0);

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            return new YubicoNeoMode(YkNeoManagerGetMode(_currentDevice));
        }

        public void SetMode(YubicoNeoModeEnum mode)
        {
            if (_currentDevice == IntPtr.Zero)
                return;

            if (_currentDevice == IntPtr.Zero)
                throw new Exception("Not initialized");

            YubicoNeoReturnCode code = YkNeoManagerSetMode(_currentDevice, mode);

            if (code != YubicoNeoReturnCode.YKNEOMGR_OK)
                throw new Exception("Unable to set mode: " + code);
        }

        public IEnumerable<string> ListDevices()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int len = 65 * 1024;
                ptr = Marshal.AllocHGlobal(len);

                YubicoNeoReturnCode res = YkNeoManagerListDevices(_currentDevice, ptr, ref len);
                if (res != YubicoNeoReturnCode.YKNEOMGR_OK)
                    yield break;

                byte[] data = new byte[len];
                Marshal.Copy(ptr, data, 0, len);

                int prev = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != 0)
                        continue;

                    yield return Encoding.ASCII.GetString(data, prev, i - prev);
                    prev = i + 1;
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public YubicoNeoReturnCode Connect(string name)
        {
            byte[] data = new byte[Encoding.ASCII.GetByteCount(name)];
            Encoding.ASCII.GetBytes(name, 0, name.Length, data, 0);

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(data.Length + 1);
                Marshal.Copy(data, 0, ptr, data.Length);

                return YkNeoManagerConnect(_currentDevice, ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public bool RefreshDevice()
        {
            if (_currentDevice == IntPtr.Zero)
                return false;

            try
            {
                YubicoNeoReturnCode res = YkNeoManagerDiscover(_currentDevice);

                if (res == YubicoNeoReturnCode.YKNEOMGR_OK)
                    return true;

                if (res == YubicoNeoReturnCode.YKNEOMGR_NO_DEVICE)
                    return false;

                if (res == YubicoNeoReturnCode.YKNEOMGR_BACKEND_ERROR)
                {
                    Close();
                    Init();

                    res = YkNeoManagerDiscover(_currentDevice);

                    if (res == YubicoNeoReturnCode.YKNEOMGR_OK)
                        return true;

                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}