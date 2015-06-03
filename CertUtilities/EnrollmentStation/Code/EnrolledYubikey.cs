using System;
using System.Xml.Serialization;

namespace EnrollmentStation.Code
{
    [Serializable]
    public class EnrolledYubikey
    {
        private string _managementKey;

        [XmlAttribute]
        public int DeviceSerial { get; set; }

        [XmlAttribute]
        public string Username { get; set; }

        [XmlAttribute]
        public string CA { get; set; }

        [XmlAttribute]
        public string ManagementKey
        {
            get { return _managementKey; }
            set
            {
                _managementKey = value;

                ManagementKeyBytes = Utilities.StringToByteArray(value);
            }
        }

        [XmlIgnore]
        public byte[] ManagementKeyBytes { get; set; }

        [XmlAttribute]
        public string Chuid { get; set; }

        [XmlAttribute]
        public string PukKey { get; set; }

        [XmlAttribute]
        public DateTime EnrolledAt { get; set; }

        public YubikeyVersions YubikeyVersions { get; set; }

        public CertificateDetails Certificate { get; set; }

        public EnrolledYubikey()
        {
            YubikeyVersions = new YubikeyVersions();
            Certificate = new CertificateDetails();
        }
    }
}