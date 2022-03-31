using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{
    [DependsOn(
        typeof(NotificacionDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule)
    )]
    public class NotificacionEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<NotificacionDbContext>(options =>
            {
                 
                options.AddDefaultRepositories<INotificacionDbContext>();
                //options.AddDefaultRepositories(includeAllEntities: true);

                options.AddRepository<Plantilla, EfCorePlantillaRepository>();
            
            });

            //Sin prefijos las tablas, de los modulos abp
            AbpSettingManagementDbProperties.DbTablePrefix = string.Empty;
            
        }
    }
}