using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
         typeof(AbpSettingManagementApplicationContractsModule)
        )]
    public class NotificacionApplicationContractsModule : AbpModule
    {

    }
}
