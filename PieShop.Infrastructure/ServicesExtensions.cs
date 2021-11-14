/*
 * Created By  	: Md. Mozaffar Rahman Sebu
 * Created Date	: July,1,2021
 * Updated By  	: Md. Mozaffar Rahman Sebu
 * Updated Date	: July,25,2021
 * (c) Datavanced LLC
 */

using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PieShop.Database;
using PieShop.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace PieShop.Infrastructure
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //add AppConfig configuration parameters
            var config = services.ConfigureStartupConfig<AppConfig>(configuration.GetSection("Profile"));
            //Initialize the project
            Initialize(services);
            services.AddSqlDbContext(configuration, config);
            services
                .AddSingleton<ICustomDbContextFactory<PieShopDbContext>, CustomDbContextFactory<PieShopDbContext>>();

            return ConfigureServices(services, configuration, config);
        }

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void Initialize(IServiceCollection services)
        {
            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CustomHubAuthorizationPolicy", policy =>
            //    {
            //        policy.Requirements.Add(new CustomAuthorizationRequirement());
            //    });
            //});
            //services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            //services.AddSignalR()
            //    .AddJsonProtocol(options => {
            //        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //    })
            //    .AddHubOptions<NotificationHub>(options =>
            //    {
            //        options.EnableDetailedErrors = true;
            //        //options.AddFilter<AdminFilterAttribute>();
            //    });
            //CQRS MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins("https://localhost:44316/", "http://localhost:4200",
                                "https://chesed.datavanced.com", "http://localhost:82/");
                    });
            });
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                    o.UseCamelCasing(processDictionaryKeys: true);
                });
            services.AddHttpClient();

            //add signalR
            //services.AddSignalR();
            //services.AddSingleton<IClient, Client>();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                                  \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                c.CustomSchemaIds(i => i.FullName);
            });

        }

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Service provider</returns>
        public static IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration, AppConfig appConfig)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.DatabaseDependencyResolver(appConfig);
            //containerBuilder.RepositoryDependencyResolver();
            //containerBuilder.ServiceDependencyResolver(appConfig);
            //containerBuilder.MapperDependencyResolver();
            //containerBuilder.FeaturesDependencyResolver(services);
            //containerBuilder.CoreDependencyResolver(appConfig);
            containerBuilder.Populate(services);
            //create service provider
            return new AutofacServiceProvider(containerBuilder.Build());
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// Register DB object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddSqlDbContext(this IServiceCollection services, IConfiguration configuration, AppConfig appConfig)
        {

            services.AddDbContext<PieShopDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Cn"));
            });
            //if (appConfig.EnableLazyLoading)
            //{
            //    services.AddDbContext<PieShopDbContext>(optionsBuilder =>
            //    {
            //        optionsBuilder.UseSqlServerWithLazyLoading(services, configuration);
            //    });
            //}
        }


        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services,
            IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }
    }


}
