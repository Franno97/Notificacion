using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class NotificationConsoleApiClientModule : AbpModule
    {
        
    }
}
