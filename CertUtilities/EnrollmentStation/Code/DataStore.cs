using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EnrollmentStation.Code
{
    public class DataStore
    {
        private List<EnrolledYubikey> _yubikeys = new List<EnrolledYubikey>();

        private DataStore(List<EnrolledYubikey> keys)
        {
            _yubikeys = keys ?? new List<EnrolledYubikey>();
        }

        public void Add(EnrolledYubikey key)
        {
            _yubikeys.Add(key);
        }

        public void Remove(EnrolledYubikey key)
        {
            _yubikeys.Remove(key);
        }

        public void Remove(string certificateSerial)
        {
            _yubikeys.RemoveAll(s => s.Certificate != null && s.Certificate.Serial == certificateSerial);
        }

        public IEnumerable<EnrolledYubikey> Search()
        {
            return _yubikeys;
        }

        public IEnumerable<EnrolledYubikey> Search(int serialNumber)
        {
            return _yubikeys.Where(s => s.DeviceSerial == serialNumber);
        }

        public static DataStore Load(string file)
        {
            if (!File.Exists(file))
                return new DataStore(null);

            XmlSerializer ser = new XmlSerializer(typeof(List<EnrolledYubikey>));

            List<EnrolledYubikey> keys;

            XDocument doc = XDocument.Load(file);
            using (XmlReader reader = doc.CreateReader())
                keys = (List<EnrolledYubikey>)ser.Deserialize(reader);

            return new DataStore(keys);
        }

        public void Save(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<EnrolledYubikey>));

            XDocument doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
                ser.Serialize(writer, _yubikeys);

            doc.Save(file);
        }
    }
}
