using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CERTCLIENTLib;
using CERTENROLLLib;

namespace PKCS10Test
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: Signer.exe [enrollmentKey] [user] [request.csr] [target.crt]");
                return;
            }

            string argsKey = args[0];
            string argsUser = args[1];
            string argsCsr = args[2];
            string argsCrt = args[3];

            const int CC_UIPICKCONFIG = 0x1;
            const int CR_IN_BASE64 = 0x1;
            const int CR_IN_FORMATANY = 0;
            const int CR_DISP_ISSUED = 0x3;
            const int CR_DISP_UNDER_SUBMISSION = 0x5;
            const int CR_OUT_BASE64 = 0x1;
            const int CR_OUT_CHAIN = 0x100;

            X509Store store = new X509Store("My", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            string csr = string.Join("\n", File.ReadAllLines(argsCsr).Where(s => s.Length > 0 && !s.StartsWith("--")));

            // Create a PKCS 10 inner request.
            CX509CertificateRequestPkcs10 pkcs10Req = new CX509CertificateRequestPkcs10();
            pkcs10Req.InitializeDecode(csr);

            // Create a CMC outer request and initialize
            CX509CertificateRequestCmc cmcReq = new CX509CertificateRequestCmc();
            cmcReq.InitializeFromInnerRequestTemplateName(pkcs10Req, "SmartcardLogon");
            cmcReq.RequesterName = argsUser;

            CSignerCertificate signer = new CSignerCertificate();
            signer.Initialize(false, X509PrivateKeyVerify.VerifyNone, (EncodingType)0xc, argsKey);
            cmcReq.SignerCertificate = signer;

            // encode the request
            cmcReq.Encode();

            string strRequest = cmcReq.RawData[EncodingType.XCN_CRYPT_STRING_BASE64];

            CCertConfig objCertConfig = new CCertConfig();
            CCertRequest objCertRequest = new CCertRequest();

            // Get CA config from UI
            string strCAConfig = objCertConfig.GetConfig(CC_UIPICKCONFIG);

            // Submit the request
            int iDisposition = objCertRequest.Submit(CR_IN_BASE64 | CR_IN_FORMATANY, strRequest, null, strCAConfig);

            // Check the submission status
            if (CR_DISP_ISSUED != iDisposition) // Not enrolled
            {
                var strDisposition = objCertRequest.GetDispositionMessage();

                if (CR_DISP_UNDER_SUBMISSION == iDisposition)
                {
                    Console.WriteLine("The submission is pending: " + strDisposition);
                    return;
                }

                Console.WriteLine("The submission failed: " + strDisposition);
                Console.WriteLine("Last status: " + objCertRequest.GetLastStatus());
                return;
            }

            // Get the certificate
            string strCert = objCertRequest.GetCertificate(CR_OUT_BASE64);

            File.WriteAllText(argsCrt, "-----BEGIN CERTIFICATE-----\n" + strCert + "-----END CERTIFICATE-----\n");
        }
    }
}
