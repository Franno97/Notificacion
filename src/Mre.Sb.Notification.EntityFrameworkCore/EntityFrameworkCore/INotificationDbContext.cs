using Microsoft.EntityFrameworkCore;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{
    [ConnectionStringName(NotificationDbProperties.ConnectionStringName)]
    public interface INotificacionDbContext : IEfCoreDbContext
    {
        
        DbSet<Plantilla> Plantillas { get; }
    }
}