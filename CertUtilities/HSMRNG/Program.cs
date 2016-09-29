using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using EnrollmentStation.Code.Utilities;

namespace HSMRNG
{
    public class Program
    {
        private const byte YSM_RANDOM_GENERATE = 0x24;
        private const byte YSM_RESPONSE = 0x80;
        private const byte YSM_MAX_PKT_SIZE = 0x60;

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: HSMRNG [NumberOfBytes]");
                return 1;
            }

            int numBytes = int.Parse(args[0]);

            if (numBytes <= 0)
            {
                Console.WriteLine("Number of bytes must be 1 or more");
                return 2;
            }

            if (numBytes > YSM_MAX_PKT_SIZE - 1)
            {
                Console.WriteLine("Number of bytes can't exceed " + (YSM_MAX_PKT_SIZE - 1));
                return 3;
            }

            SerialPort device = null;

            //Looking for YubiHSM
            List<Win32DeviceMgmt.DeviceInfo> results = Win32DeviceMgmt.GetAllCOMPorts();
            foreach (Win32DeviceMgmt.DeviceInfo deviceInfo in results)
            {
                if (deviceInfo.bus_description == "Yubico YubiHSM")
                    device = new SerialPort(deviceInfo.name);
            }

            if (device == null)
            {
                Console.WriteLine("Found no YubiHSM device.");
                return 4;
            }

            device.ReadTimeout = 5000;
            device.WriteTimeout = 5000;

            try
            {
                device.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error opening the device. Error: " + ex.Message);
                return 5;
            }

            byte[] cmdBuffer = { (byte)numBytes };
            byte[] fullCommand = new[] { (byte)(((cmdBuffer.Length + 1) << 24) >> 24), YSM_RANDOM_GENERATE }.Concat(cmdBuffer).ToArray();

            try
            {
                device.Write(fullCommand, 0, fullCommand.Length);
                Thread.Sleep(100);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed writing to the YubiHSM device. Try reconnecting it.");
                return 6;
            }

            byte[] result = new byte[2];

            try
            {
                device.Read(result, 0, 2);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed reading from the YubiHSM device. Try reconnecting it.");
                return 7;
            }

            if (result[1] != (YSM_RANDOM_GENERATE | YSM_RESPONSE))
                throw new Exception("YubiHSM returned wrong response.");

            int responseLength = result[0] - 1;
            result = new byte[responseLength];

            try
            {
                device.Read(result, 0, responseLength);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed reading from the YubiHSM device. Try reconnecting it.");
                return 8;
            }

            Console.WriteLine(Skip1ToHex(result));

            if (device.IsOpen)
                device.Close();

            return 0;
        }

        private static string Skip1ToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }
    }
}
