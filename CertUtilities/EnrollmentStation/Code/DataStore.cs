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
        public List<EnrolledYubikey> Yubikeys { get; private set; }

        private DataStore(List<EnrolledYubikey> keys)
        {
            Yubikeys = keys ?? new List<EnrolledYubikey>();
        }

        public void Add(EnrolledYubikey key)
        {
            Yubikeys.Add(key);
        }

        public void Remove(EnrolledYubikey key)
        {
            Yubikeys.Remove(key);
        }

        public void Remove(string certificateSerial)
        {
            Yubikeys.RemoveAll(s => s.Certificate != null && s.Certificate.Serial == certificateSerial);
        }

        public IEnumerable<EnrolledYubikey> Search()
        {
            return Yubikeys;
        }

        public IEnumerable<EnrolledYubikey> Search(int serialNumber)
        {
            return Yubikeys.Where(s => s.DeviceSerial == serialNumber);
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
            string bakFile = file + ".bak";

            XmlSerializer ser = new XmlSerializer(typeof(List<EnrolledYubikey>));

            XDocument doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
                ser.Serialize(writer, Yubikeys);

            // Keep a backup
            if (File.Exists(bakFile) && File.Exists(file))
            {
                File.Delete(bakFile);
                File.Move(file, bakFile);
            }
            else if (File.Exists(file))
            {
                File.Move(file, bakFile);
            }

            doc.Save(file);
        }
    }
}
