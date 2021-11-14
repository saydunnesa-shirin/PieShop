using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Database
{
    /// <summary>
    /// Custom Convention for DbContext's ModelBuilder
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// EF Core doesn't have any convention to it, we can do it by getting the entity name
        /// To do so, first we need to create an extension method on ModelBuilder object:
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName());
            }
        }
    }
}
