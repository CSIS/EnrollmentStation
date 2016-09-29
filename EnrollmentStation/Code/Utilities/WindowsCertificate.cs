using System.Security.Cryptography.X509Certificates;

namespace EnrollmentStation.Code.Utilities
{
    public class WindowsCertificate
    {
        public WindowsCertificate(StoreLocation storeLocation, X509Certificate2 certificate)
        {
            StoreLocation = storeLocation;
            Certificate = certificate;
        }

        public StoreLocation StoreLocation { get; }

        public X509Certificate2 Certificate { get; }
    }
}