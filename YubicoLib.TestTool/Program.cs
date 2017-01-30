using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using YubicoLib.YubikeyNeo;
using YubicoLib.YubikeyPiv;

namespace YubicoLib.TestTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // NEO
            List<string> devices = YubikeyNeoManager.Instance.ListDevices(false).ToList();

            Console.WriteLine($"[NEO] Found {devices.Count:N0} devices");

            foreach (string device in devices)
            {
                PrintNeo(device);
            }

            Console.WriteLine();

            // PIV
            devices = YubikeyPivManager.Instance.ListDevices(false).ToList();

            Console.WriteLine($"[PIV] Found {devices.Count:N0} devices");

            foreach (string device in devices)
            {
                PrintPiv(device);
            }

            Console.WriteLine();
        }

        static void PrintNeo(string name)
        {
            Console.WriteLine($"[NEO] Device: {name}");

            if (!YubikeyNeoManager.Instance.IsValidDevice(name))
            {
                Console.WriteLine("      Not a valid NEO device");
            }
            else
            {
                using (YubikeyNeoDevice device = YubikeyNeoManager.Instance.OpenDevice(name))
                {
                    Console.WriteLine($"      Serial  : {device.GetSerialNumber()}");
                    Console.WriteLine($"      Mode    : {device.GetMode()}");
                    Console.WriteLine($"      Version : {device.GetVersion()}");
                }
            }

            Console.WriteLine();
        }

        static void PrintPiv(string name)
        {
            Console.WriteLine($"[PIV] Device: {name}");

            if (!YubikeyPivManager.Instance.IsValidDevice(name))
            {
                Console.WriteLine("      Not a valid PIV device");
            }
            else
            {
                using (YubikeyPivDevice device = YubikeyPivManager.Instance.OpenDevice(name))
                {
                    Console.WriteLine($"      Version : {device.GetVersion()}");

                    byte[] chuid;
                    if (device.GetCHUID(out chuid))
                        Console.WriteLine($"      CHUID   : {BitConverter.ToString(chuid).Replace("-", "")}");
                    else
                        Console.WriteLine("      CHUID   : N/A");

                    Console.WriteLine($"      PinTries: {device.GetPinTriesLeft():N0}");

                    X509Certificate2 cert = device.GetCertificate9a();

                    if (cert != null)
                    {
                        Console.WriteLine($"      Cert 9A, Subject: {cert.SubjectName}");
                        Console.WriteLine($"               Issuer : {cert.IssuerName}");
                        Console.WriteLine($"               Start  : {cert.NotBefore.ToUniversalTime():O}");
                        Console.WriteLine($"               Expiry : {cert.NotAfter.ToUniversalTime():O}");
                        Console.WriteLine($"               Serial : {cert.SerialNumber}");
                        Console.WriteLine($"               Finger : {cert.Thumbprint}");
                    }
                    else
                        Console.WriteLine("      Cert 9A : N/A");
                }
            }

            Console.WriteLine();
        }
    }
}
