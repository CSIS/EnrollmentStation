using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Threading;

namespace EnrollmentStation.Code
{
    public static class HsmRng
    {
        private const byte YSM_RANDOM_GENERATE = 0x24;
        private const byte YSM_RESPONSE = 0x80;
        private const byte YSM_MAX_PKT_SIZE = 0x60;

        private static SerialPort FindDevice()
        {
            //Looking for YubiHSM
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                List<ManagementBaseObject> ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                foreach (ManagementBaseObject obj in ports)
                {
                    string deviceId = obj["DeviceID"].ToString();
                    string caption = obj["Caption"].ToString();

                    if (caption.Contains("Yubico YubiHSM"))
                        return new SerialPort(deviceId);
                }
            }

            return null;
        }

        public static bool IsHsmPresent()
        {
            return FindDevice() != null;
        }

        public static byte[] FetchRandom(int numBytes)
        {
            if (numBytes <= 0)
                throw new ArgumentOutOfRangeException("numBytes", "Number of bytes must be 1 or more");

            if (numBytes > YSM_MAX_PKT_SIZE - 1)
                throw new ArgumentOutOfRangeException("numBytes", "Number of bytes can't exceed " + (YSM_MAX_PKT_SIZE - 1));

            SerialPort device = FindDevice();

            if (device == null)
                throw new InvalidOperationException("A YubiHSM device was not present");

            device.ReadTimeout = 5000;
            device.WriteTimeout = 5000;

            try
            {
                device.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Was unable to open the YubiHSM device", ex);
            }

            byte[] cmdBuffer = { (byte)numBytes };
            byte[] fullCommand = new[] { (byte)(((cmdBuffer.Length + 1) << 24) >> 24), YSM_RANDOM_GENERATE }.Concat(cmdBuffer).ToArray();

            try
            {
                device.Write(fullCommand, 0, fullCommand.Length);
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed writing to the YubiHSM device. Try reconnecting it.", ex);
            }

            byte[] result = new byte[2];

            try
            {
                device.Read(result, 0, 2);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed reading from the YubiHSM device. Try reconnecting it.", ex);
            }

            if (result[1] != (YSM_RANDOM_GENERATE | YSM_RESPONSE))
                throw new Exception("YubiHSM returned wrong response.");

            int responseLength = result[0] - 1;
            result = new byte[responseLength];

            try
            {
                device.Read(result, 0, responseLength);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed reading from the YubiHSM device. Try reconnecting it.", ex);
            }

            if (device.IsOpen)
                device.Close();

            byte[] allExceptFirst = new byte[result.Length - 1];
            Array.Copy(result, 0, allExceptFirst, 0, allExceptFirst.Length);

            return allExceptFirst;
        }
    }
}
