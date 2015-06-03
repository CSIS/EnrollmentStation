using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EnrollmentStation.Code
{
    public class Settings
    {
        private Settings()
        {
        }

        public string CSREndpoint { get; set; }

        public string EnrollmentAgentCertificate { get; set; }

        public string EnrollmentManagementKey { get; set; }

        public string EnrollmentCaTemplate { get; set; }

        public static Settings Load(string file)
        {
            if (!File.Exists(file))
                return new Settings();

            XmlSerializer ser = new XmlSerializer(typeof(Settings));
            XDocument doc = XDocument.Load(file);

            using (XmlReader reader = doc.CreateReader())
                return (Settings)ser.Deserialize(reader);
        }

        public void Save(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Settings));
            XDocument doc = new XDocument();

            using (XmlWriter writer = doc.CreateWriter())
                ser.Serialize(writer, this);

            doc.Save(file);
        }
    }
}