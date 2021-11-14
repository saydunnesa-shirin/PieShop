using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Utility
{
    public class AppConfig
    {
        /// <summary>
        /// Enable Lazy-Loading for EF Core
        /// </summary>
        public bool EnableLazyLoading { get; set; }
        /// <summary>
        /// Enable/Disable Table Name in Plural/Singular format
        /// </summary>
        public bool PluralizeTableName { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        /// <summary>
        /// Gets or sets Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the data protection system should be configured to persist keys in the Redis database
        /// </summary>
        public bool PersistDataProtectionKeysToRedis { get; set; }

        public bool UseRowNumberForPaging { get; set; }
    }
}
