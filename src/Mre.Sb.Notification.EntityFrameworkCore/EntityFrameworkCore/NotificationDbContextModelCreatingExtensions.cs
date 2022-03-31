using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{
    public static class NotificationDbContextModelCreatingExtensions
    {
        public static void ConfigurarNotificaciones(
            this ModelBuilder builder,
            Action<NotificationModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new NotificationModelBuilderConfigurationOptions(
                NotificationDbProperties.DbTablePrefix,
                NotificationDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

           
            builder.Entity<Plantilla>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Plantilla", options.Schema);
            
                b.ConfigureByConvention();
             
            });
             
        }
    }
}