using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionCare.Utility.Helpers
{
    public class AppDictionary
    {
        public static ConcurrentDictionary<string, string> GetOrganizationDictionary()
        {
            ConcurrentDictionary<string, string> organizations = new ConcurrentDictionary<string, string>();
            organizations.TryAdd("Datavanced", "PassionCare");
            return organizations;
        }

        public string GetDbNameByOrganization(string org)
        {
            Dictionary<string, string> organizations = new Dictionary<string, string>();
            organizations.Add("Datavanced", "PassionCare");
            return null;
        }
    }
}
