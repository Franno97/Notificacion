using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{
    public class EfCorePlantillaRepository : EfCoreRepository<INotificacionDbContext, Plantilla, string>,
        IPlantillaRepository
    {
        public EfCorePlantillaRepository(IDbContextProvider<INotificacionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        
        public virtual async Task<List<Plantilla>> ObtenerListaAsync(
            string channelId,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .ToListAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<bool> ExisteAsync(string plantillaCodigo, CancellationToken cancellationToken = default)
        {
            var query = await GetDbSetAsync();
            return await query.AnyAsync(a => a.Id == plantillaCodigo);

        }

    }
}