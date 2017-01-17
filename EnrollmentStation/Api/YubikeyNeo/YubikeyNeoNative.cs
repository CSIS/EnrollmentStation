using System;
using System.Runtime.InteropServices;
using EnrollmentStation.Code.Enums;

namespace EnrollmentStation.Api.YubikeyNeo
{
    internal static class YubikeyNeoNative
    {
        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_global_init", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerGlobalInit(int initFlags);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_global_done", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void YkNeoManagerGlobalDone();

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_init", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerInit(ref IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_done", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void YkNeoManagerDone(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_mode", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoModeEnum YkNeoManagerGetMode(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_modeswitch", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerSetMode(IntPtr dev, YubicoNeoModeEnum mode);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_serialno", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int YkNeoManagerGetSerialNumber(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_discover", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerDiscover(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_discover_match", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerDiscoverMatch(IntPtr dev, IntPtr match);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_connect", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerConnect(IntPtr dev, string name);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_version_major", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte YkNeoManagerGetVersionMajor(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_version_minor", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte YkNeoManagerGetVersionMinor(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_get_version_build", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte YkNeoManagerGetVersionBuild(IntPtr dev);

        [DllImport("Binaries\\libykneomgr-0.dll", EntryPoint = "ykneomgr_list_devices", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern YubicoNeoReturnCode YkNeoManagerListDevices(IntPtr dev, IntPtr buffer, ref int length);
    }
}