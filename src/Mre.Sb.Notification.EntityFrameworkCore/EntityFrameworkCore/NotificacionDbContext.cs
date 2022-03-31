using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{

    [ReplaceDbContext(typeof(ISettingManagementDbContext))]
    [ConnectionStringName(NotificationDbProperties.ConnectionStringName)]
    public class NotificacionDbContext : AbpDbContext<NotificacionDbContext>
        , ISettingManagementDbContext
        , INotificacionDbContext
    {
        public DbSet<Setting> Settings { get; }

        public DbSet<Plantilla> Plantillas { get; }

        public NotificacionDbContext(DbContextOptions<NotificacionDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
            builder.ConfigurarNotificaciones();

            builder.ConfigurarGestionConfiguracion();

        }
    }

    public static class GestionConfiguracionDbContextModelBuilderExtensions
    {
        public static void ConfigurarGestionConfiguracion(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            builder.Entity<Setting>(b =>
            {
                b.ToTable(AbpSettingManagementDbProperties.DbTablePrefix + "Configuracion", AbpSettingManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasColumnName("Nombre").HasMaxLength(SettingConsts.MaxNameLength).IsRequired();

                if (builder.IsUsingOracle()) { SettingConsts.MaxValueLengthValue = 2000; }
                b.Property(x => x.Value).HasColumnName("Valor").HasMaxLength(SettingConsts.MaxValueLengthValue).IsRequired();

                b.Property(x => x.ProviderName).HasColumnName("Proveedor").HasMaxLength(SettingConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasColumnName("ProveedorClave").HasMaxLength(SettingConsts.MaxProviderKeyLength);

                b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey }).IsUnique(true);

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<SettingManagementDbContext>();
        }
    }
}