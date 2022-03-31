using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mre.Sb.Notificacion
{
    public interface IPlantillaRepository :  IRepository<Plantilla, string>  //IBasicRepository<Plantilla, string>,
    {
        Task<List<Plantilla>> ObtenerListaAsync(string channelId,
            CancellationToken cancellationToken = default);

        Task<bool> ExisteAsync(string plantillaCodigo,
           CancellationToken cancellationToken = default);

    }

}
