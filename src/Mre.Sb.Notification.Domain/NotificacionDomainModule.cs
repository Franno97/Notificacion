using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(NotificacionDomainSharedModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class NotificacionDomainModule : AbpModule
    {

    }
}
