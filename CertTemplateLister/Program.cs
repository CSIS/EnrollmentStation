using System;
using System.DirectoryServices;

namespace CertTemplateLister
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryEntry rootDse = new DirectoryEntry("LDAP://RootDSE");
            string configNamingContext = rootDse.Properties["configurationNamingContext"].Value.ToString();

            DirectoryEntry certTemplates = new DirectoryEntry("LDAP://CN=Certificate Templates,CN=Public Key Services,CN=Services," + configNamingContext);
            DirectorySearcher templatesSearch = new DirectorySearcher(certTemplates, "(objectClass=pKICertificateTemplate)", null, SearchScope.OneLevel);

            SearchResultCollection templates = templatesSearch.FindAll();

            foreach (SearchResult template in templates)
            {
                Console.WriteLine($"Name: {template.Properties["name"][0]} ({template.Properties["displayName"][0]})");
                Console.WriteLine($"Flags: {template.Properties["msPKI-Enrollment-Flag"][0]}");
                Console.WriteLine("");
            }

        }
    }
}
