using Autofac;
using Microsoft.EntityFrameworkCore;
using PieShop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Database
{
    public static class DatabaseDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        public static void DatabaseDependencyResolver(this ContainerBuilder builder, AppConfig config)
        {
            //data layer
            builder.Register(context => new PieShopDbContext(context.Resolve<DbContextOptions<PieShopDbContext>>())).InstancePerLifetimeScope();
            //builder.RegisterType<HttpRequestHelper>().As<IHttpRequestHelper>().InstancePerLifetimeScope();
        }
    }
}
