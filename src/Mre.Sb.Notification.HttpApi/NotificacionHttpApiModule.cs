using Localization.Resources.AbpUi;
using Mre.Sb.Notificacion.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        ,typeof(AbpSettingManagementHttpApiModule)
        )]
    public class NotificacionHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(NotificacionHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
          

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<NotificationResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
