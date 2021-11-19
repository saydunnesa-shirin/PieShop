using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionCare.Utility.Helpers
{
    public class LocalStoreMapper
    {
        private static readonly Lazy<LocalStoreMapper> lazy = new Lazy<LocalStoreMapper>(() => new LocalStoreMapper());

        private static ConcurrentDictionary<string, string> DbInstances;

        public static LocalStoreMapper Instance { get { return lazy.Value; } }

        private LocalStoreMapper()
        {
            DbInstances = AppDictionary.GetOrganizationDictionary();
        }

        public string SetDbInstance(string id, string value)
        {
            return DbInstances.AddOrUpdate(id, value, (oldkey, oldvalue) => value);
        }

        public string GetDbInstance(string id)
        {
            string value;
            return DbInstances.TryGetValue(id, out value) ? value : null;
        }

        public bool RemoveDbInstance(string id)
        {
            string value;
            return DbInstances.TryRemove(id, out value);
        }

        public List<string> GetAllDbInstances()
        {
            return DbInstances.Values.ToList();
        }
    }
}
