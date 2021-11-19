/*
 * Created By  	: Md. Mozaffar Rahman Sebu
 * Created Date	: Nov,19,2021
 * Updated By  	: Md. Mozaffar Rahman Sebu
 * Updated Date	: Nov,19,2021
 * (c) Datavanced LLC
 */

using Autofac;
using Autofac.Core;
using System;
using System.Linq;
using System.Reflection;

namespace PieShop.Mappers
{
    public static class MapperDependencyRegistrar
    {
        /// <summary>
        /// Register Mapper and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        public static void MapperDependencyResolver(this ContainerBuilder builder)
        {
            new AutoFacReg().LoadMappers(builder);
        }
    }

    public class AutoFacReg : Autofac.Module
    {
        public void LoadMappers(ContainerBuilder builder)
        {
            Load(builder);
        }
        protected override void Load(ContainerBuilder builder)
        {
            RegisterHandlers(builder, typeof(IMapper<,>));
            base.Load(builder);
        }

        private static void RegisterHandlers(ContainerBuilder builder, Type handlerType, params Type[] decorators)
        {
            RegisterHandlers(builder, handlerType);
            for (var i = 0; i < decorators.Length; i++)
            {
                RegisterGenericDecorator(builder, decorators[i], handlerType, i == 0 ? handlerType : decorators[i - 1], i != decorators.Length - 1);
            }
        }
        private static void RegisterHandlers(ContainerBuilder builder, Type handlerType)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IMapper<,>))).As(t =>
            {
                var c = t.GetInterfaces().Where(v => v.IsClosedTypeOf(handlerType)).Select(v => new KeyedService(handlerType.Name, v));
                return c;
            }).AsImplementedInterfaces();
        }

        private static void RegisterGenericDecorator(ContainerBuilder builder, Type decoratorType, Type serviceType, Type fromKeyType, bool isKey)
        {
            var r = builder.RegisterGenericDecorator(decoratorType, serviceType, fromKeyType.Name).InstancePerLifetimeScope();
            if (isKey)
            {
                r.Keyed(decoratorType.Name, serviceType);
            }
        }
    }
}
