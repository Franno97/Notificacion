using Mre.Sb.Notificacion.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Mre.Sb.Notificacion
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(NotificationEntityFrameworkCoreTestModule)
        )]
    public class NotificationDomainTestModule : AbpModule
    {
        
    }
}
