using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mre.Sb.Notificacion
{
    public interface IPlantillaStore
    {
        Task<Plantilla> ObtenerAsync(string templateId);

        Task<List<Plantilla>> ObtenerListaAsync(string channelId);

    }
}
