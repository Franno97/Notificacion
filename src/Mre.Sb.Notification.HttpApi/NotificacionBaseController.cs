using Mre.Sb.Notificacion.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Mre.Sb.Notificacion
{
    public abstract class NotificacionBaseController : AbpController
    {
        protected NotificacionBaseController()
        {
            LocalizationResource = typeof(NotificationResource);
        }
    }
}
