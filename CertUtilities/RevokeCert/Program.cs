// IF COMPILING ON WINDOWS 8 OR NEWER, IF NOT COMMENT IT OUT
//#define WIN8_COMPILE

using System;
using System.Runtime.InteropServices;
using CERTADMINLib;

#if WIN8_COMPILE
using CERTCLILib;
#else
using CERTCLIENTLib;
#endif

namespace RevokeCert
{
    class Program
    {
        private const int CC_UIPICKCONFIG = 0x1;

        static int Main(string[] args)
        {
            string caConfig;
            int reason;
            string serial;

            if (args.Length == 1)
            {
                CCertConfig objCertConfig = new CCertConfig();
                caConfig = objCertConfig.GetConfig(CC_UIPICKCONFIG);

                reason = (int)RevokeReason.CRL_REASON_CESSATION_OF_OPERATION;
                serial = args[0];
            }
            else if (args.Length == 3)
            {
                caConfig = args[0];
                reason = int.Parse(args[1]);
                serial = args[2];
            }
            else
            {
                Console.WriteLine("Usage: RevokeCert.exe [SerialNumber]");
                Console.WriteLine("Usage: RevokeCert.exe [CAConfig] [Reason] [SerialNumber]");
                return 2;
            }

            CCertAdmin admin = null;
            try
            {
                admin = new CCertAdmin();
                admin.RevokeCertificate(caConfig, serial, reason, DateTime.Now);

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
            finally
            {
                if (admin != null)
                    Marshal.FinalReleaseComObject(admin);
            }
        }
    }
}