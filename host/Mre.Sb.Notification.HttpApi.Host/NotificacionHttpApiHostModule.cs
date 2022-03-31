using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mre.Sb.Notificacion.EntityFrameworkCore;
using Mre.Sb.Notificacion.MultiTenancy;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using Mre.Sb.Notificacion.Grpc;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.EventBus.Kafka;
using Mre.Sb.PermisoRemoto.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.EventBus;
using Microsoft.Extensions.Configuration;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionApplicationModule),
        typeof(NotificacionEntityFrameworkCoreModule),
        typeof(NotificacionHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    [DependsOn(typeof(AbpEventBusKafkaModule))]
    [DependsOn(
        typeof(PermisoRemotoAbpModule)
        )]
    public class NotificacionHttpApiHostModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            PreConfigure<AbpEventBusOptions>(options =>
            {
                options.EnabledErrorHandle = true;
                options.UseRetryStrategy(retryStrategyOptions =>
                {
                    retryStrategyOptions.IntervalMillisecond = configuration.GetValue<int>("EventosDistribuidos:IntervaloTiempo");
                    retryStrategyOptions.MaxRetryAttempts = configuration.GetValue<int>("EventosDistribuidos:NumeroReintentos");
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            context.Services.AddSameSiteCookiePolicy();

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                { 
                    //options.FileSets.ReplaceEmbeddedByPhysical<NotificationDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mre.Sb.Notification.Domain.Shared", Path.DirectorySeparatorChar)));
                    //options.FileSets.ReplaceEmbeddedByPhysical<NotificationDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mre.Sb.Notification.Domain", Path.DirectorySeparatorChar)));
                    //options.FileSets.ReplaceEmbeddedByPhysical<NotificationApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mre.Sb.Notification.Application.Contracts", Path.DirectorySeparatorChar)));
                    //options.FileSets.ReplaceEmbeddedByPhysical<NotificationApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Mre.Sb.Notification.Application", Path.DirectorySeparatorChar)));
                });
            }

            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"Notificacion", "Notificacion API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Notificaciones API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            Configure<AbpLocalizationOptions>(options =>
            {
                //options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
            });

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = configuration["AuthServer:Audience"];
                });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = configuration["Cache:KeyPrefix"];
            });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "Notification-Protection-Keys");
            }

            //Auto API Controllers
            //Configure<AbpAspNetCoreMvcOptions>(options =>
            //{
            //    options
            //        .ConventionalControllers
            //        .Create(typeof(NotificationApplicationModule).Assembly);
            //});

             

            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabled = false; //Desactivar auditoria abp
            });


            //Configurar Notificaciones
            Configure<NotificacionConfiguracion>(
                 configuration.GetSection("Notificaciones"));
             
            ConfigureGrpcServices(context.Services);

            
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseCookiePolicy();


            app.UseAuthentication();
            //if (MultiTenancyConsts.IsEnabled)
            //{
            //    app.UseMultiTenancy();
            //}
            app.UseAbpRequestLocalization();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("Notificacion");
            });

        
            app.UseAbpSerilogEnrichers();
            
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<NotificationService>();

                //TODO: Revisar
                if (env.IsDevelopment())
                {
                   endpoints.MapGrpcReflectionService();
                }
            });

           

        }

        private void ConfigureGrpcServices(IServiceCollection services)
        {
            services.AddGrpc();

            //TODO: Revisar
            services.AddGrpcReflection();
        }
        
    }

    public static class AbpApplicationBuilderErrorPageExtensions
    {
        public static IApplicationBuilder UseErrorPage(this IApplicationBuilder app)
        {
            return app
                .UseStatusCodePagesWithRedirects("~/Error?httpStatusCode={0}")
                .UseExceptionHandler("/Error");
        }
    }
}
