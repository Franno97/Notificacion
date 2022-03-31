using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Mre.Sb.Notificacion.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(AbpValidationModule),
        typeof(AbpSettingManagementDomainSharedModule)
    )]
    public class NotificacionDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<NotificacionDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<NotificationResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Notification");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Notification", typeof(NotificationResource));
            });
        }
    }
}
