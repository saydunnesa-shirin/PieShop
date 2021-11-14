using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Database
{
    public class CustomDbContextFactory<T> : ICustomDbContextFactory<T> where T : DbContext
    {
        public IConfiguration _configuration;
        public CustomDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public T CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(string.IsNullOrEmpty(connectionString) ? _configuration.GetConnectionString("Cn") : connectionString);
            return Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
        }
    }

    public interface ICustomDbContextFactory<out T> where T : DbContext
    {
        T CreateDbContext(string connectionString);
    }
}
