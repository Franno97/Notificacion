using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.Notificacion
{
    public interface INotificadorAppService : IApplicationService
    {
        Task<bool> EnviarAsync(NotificacionMensajeDto notificationMessage);

        Task<bool> EnviarConAdjuntosAsync(
            IList<IFormFile> adjuntos,
            string codigo,
            string asunto,
            string destinatarios,
            Dictionary<string, string> model);

    }
}
