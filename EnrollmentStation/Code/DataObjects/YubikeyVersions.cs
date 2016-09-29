using System.Xml.Serialization;

namespace EnrollmentStation.Code.DataObjects
{
    public class YubikeyVersions
    {
        [XmlAttribute]
        public string NeoFirmware { get; set; }

        [XmlAttribute]
        public string PivApplet { get; set; }
    }
}