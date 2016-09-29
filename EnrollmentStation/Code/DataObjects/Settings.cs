using System.IO;
using Newtonsoft.Json;

namespace EnrollmentStation.Code.DataObjects
{
    public class Settings
    {
        private Settings()
        {
        }

        public string CSREndpoint { get; set; }

        public string EnrollmentAgentCertificate { get; set; }

        public byte[] EnrollmentManagementKey { get; set; }

        public string EnrollmentCaTemplate { get; set; }

        public byte DefaultAlgorithm { get; set; }

        public static Settings Load(string file)
        {
            if (!File.Exists(file))
                return new Settings();

            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(file)) ?? new Settings();
        }

        public void Save(string file)
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(this));
        }
    }
}