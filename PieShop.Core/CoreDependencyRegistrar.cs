using Autofac;
using PieShop.Core.SignalR;
using PieShop.Utility;
using System;

namespace PieShop.Core
{
    public static class CoreDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        public static void CoreDependencyResolver(this ContainerBuilder builder, AppConfig config)
        {
            builder.RegisterGeneric(typeof(NotificationHubServices<>)).As(typeof(INotificationHubServices<>)).InstancePerLifetimeScope();
            
        }
    }
}
