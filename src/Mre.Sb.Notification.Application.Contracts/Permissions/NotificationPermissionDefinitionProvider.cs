using Mre.Sb.Notificacion.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Mre.Sb.Notificacion.Permissions
{
    public class NotificacionPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<NotificationResource>(name);
        }
    }
}