using Mre.Sb.Notificacion.Localization;
using Volo.Abp.Application.Services;

namespace Mre.Sb.Notificacion
{
    public abstract class NotificacionBaseAppService : ApplicationService
    {
        protected NotificacionBaseAppService()
        {
            LocalizationResource = typeof(NotificationResource);
            ObjectMapperContext = typeof(NotificacionApplicationModule);
        }
    }
}
