using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Mre.Sb.Notificacion
{
    [DependsOn(
        typeof(NotificacionApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class NotificacionHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Notification";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(NotificacionApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
