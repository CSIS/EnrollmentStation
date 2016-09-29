using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EnrollmentStation.Code.Utilities
{
    public static class WindowsCertStoreUtilities
    {
        public static IEnumerable<WindowsCertificate> GetAgentCertificates()
        {
            return IterateStores(IsAgentCertificate);
        }

        public static WindowsCertificate FindCertificate(string thumbPrint)
        {
            return IterateStores(s => s.Thumbprint == thumbPrint).FirstOrDefault();
        }


        private static IEnumerable<WindowsCertificate> IterateStores(Func<X509Certificate2, bool> filter)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (filter(certificate))
                        yield return new WindowsCertificate(StoreLocation.CurrentUser, certificate);
                }
            }
            finally
            {
                store.Close();
            }

            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (filter(certificate))
                        yield return new WindowsCertificate(StoreLocation.LocalMachine, certificate);
                }
            }
            finally
            {
                store.Close();
            }
        }

        public static bool IsAgentCertificate(X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
                return false;

            // Enhanced Key Usage is 2.5.29.37
            X509EnhancedKeyUsageExtension ekuExtension = null;
            foreach (X509Extension extension in cert.Extensions)
            {
                if (extension.Oid.Value == "2.5.29.37")
                    ekuExtension = (X509EnhancedKeyUsageExtension)extension;
            }

            if (ekuExtension == null)
                return false;

            // Certificate Request Agent is 1.3.6.1.4.1.311.20.2.1
            foreach (Oid oid in ekuExtension.EnhancedKeyUsages)
            {
                if (oid.Value == "1.3.6.1.4.1.311.20.2.1")
                    return true;
            }

            return false;
        }
    }
}