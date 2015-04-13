using System;
using System.Runtime.InteropServices;
using CERTADMINLib;
using CERTCLIENTLib;

namespace RevokeCert
{
    class Program
    {
        private const int CC_UIPICKCONFIG = 0x1;

        private enum RevokeReason
        {
            CRL_REASON_UNSPECIFIED = 0,
            CRL_REASON_KEY_COMPROMISE = 1,
            CRL_REASON_CA_COMPROMISE = 2,
            CRL_REASON_AFFILIATION_CHANGED = 3,
            CRL_REASON_SUPERSEDED = 4,
            CRL_REASON_CESSATION_OF_OPERATION = 5,
            CRL_REASON_CERTIFICATE_HOLD = 6
        }

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: RevokeCert.exe [SerialNumber]");
                return 2;
            }

            string serial = args[0];

            CCertConfig objCertConfig = new CCertConfig();
            string strCAConfig = objCertConfig.GetConfig(CC_UIPICKCONFIG);

            bool success = RevokeCert(strCAConfig, serial);

            return success ? 0 : 1;
        }

        private static bool RevokeCert(string config, string serial)
        {
            //config= "192.168.71.128\\My-CA"
            //serial = "614870cd000000000014"

            CCertAdmin admin = null;
            try
            {
                admin = new CCertAdmin();
                admin.RevokeCertificate(config, serial, (int)RevokeReason.CRL_REASON_CESSATION_OF_OPERATION, DateTime.Now);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (admin != null)
                    Marshal.FinalReleaseComObject(admin);
            }
        }
    }
}