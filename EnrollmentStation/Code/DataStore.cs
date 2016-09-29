using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnrollmentStation.Code.DataObjects;
using Newtonsoft.Json;

namespace EnrollmentStation.Code
{
    public class DataStore
    {
        public List<EnrolledYubikey> Yubikeys { get; private set; }

        private DataStore()
        {
            Yubikeys = new List<EnrolledYubikey>();
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
                return new DataStore();

            return JsonConvert.DeserializeObject<DataStore>(File.ReadAllText(file));
        }

        public void Save(string file)
        {
            string bakFile = file + ".bak";

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

            File.WriteAllText(file, JsonConvert.SerializeObject(this));
        }
    }
}
