using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{
    public class NotificacionDbContextFactory : IDesignTimeDbContextFactory<NotificacionDbContext>
    {
        public NotificacionDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<NotificacionDbContext>()
                .UseSqlServer(configuration.GetConnectionString(NotificationDbProperties.ConnectionStringName));

            return new NotificacionDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
           
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../host/Mre.Sb.Notification.HttpApi.Host/"))

                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}