using Volo.Abp.Modularity;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionApplicationModule),
        typeof(NotificationDomainTestModule)
        )]
    public class NotificationApplicationTestModule : AbpModule
    {

    }
}
