using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Infrastructure
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            // Make sure the CORS middleware is ahead of SignalR.
            application.UseCors("CorsPolicy");

            application.UseSwagger();
            application.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
            });

            application.UseRouting();
            application.UseAuthorization();
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<NotificationHub>("/negotiate", options =>
                //{
                //    options.Transports =
                //        HttpTransportType.WebSockets |
                //        HttpTransportType.LongPolling;
                //});
            });
            application.UseMvc();
        }
    }
}
