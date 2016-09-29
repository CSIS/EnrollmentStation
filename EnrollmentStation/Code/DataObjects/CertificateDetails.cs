using System;
using System.Xml.Serialization;

namespace EnrollmentStation.Code.DataObjects
{
    public class CertificateDetails
    {
        [XmlAttribute]
        public string Serial { get; set; }

        [XmlAttribute]
        public string Thumbprint { get; set; }

        [XmlAttribute]
        public string Subject { get; set; }

        [XmlAttribute]
        public string Issuer { get; set; }

        [XmlAttribute]
        public DateTime StartDate { get; set; }

        [XmlAttribute]
        public DateTime ExpireDate { get; set; }

        [XmlElement(DataType = "hexBinary")]
        public byte[] RawCertificate { get; set; }
    }
}