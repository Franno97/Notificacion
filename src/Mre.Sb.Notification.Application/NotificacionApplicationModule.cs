using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.VirtualFileSystem;
using System;
using Volo.Abp.MailKit;
using MailKit.Security;
using Volo.Abp.SettingManagement;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Settings;
using Volo.Abp;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionDomainModule),
        typeof(NotificacionApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        , typeof(AbpSettingManagementApplicationModule)
        )]
    [DependsOn(typeof(AbpEmailingModule))]
    [DependsOn(typeof(AbpTextTemplatingScribanModule))]

    [DependsOn(typeof(AbpMailKitModule))] 
    public class NotificacionApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<NotificacionApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<NotificacionApplicationModule>(validate: true);
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<NotificacionApplicationModule>();
            });


            var configuration = context.Services.GetConfiguration();

             var enviarEmailNotificaciones =  Convert.ToBoolean(configuration["Notificaciones:EnviarEmail"]);

            if (!enviarEmailNotificaciones) {
                context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
            }

            var secureSocketOptions = SecureSocketOptions.None;
            Enum.TryParse(configuration["Notificaciones:MailKit:SecureSocketOptions"],out secureSocketOptions);

           
            Configure<AbpMailKitOptions>(options =>
            {
                options.SecureSocketOption = secureSocketOptions;

            });

       

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.AutoEventSelectors.Add<Plantilla>();
                options.EtoMappings.Add<Plantilla, PlantillaEto>();

                 
            }); 

        }

        
    }
}
