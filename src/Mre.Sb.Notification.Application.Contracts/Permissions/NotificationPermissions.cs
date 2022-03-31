using Volo.Abp.Reflection;

namespace Mre.Sb.Notificacion.Permissions
{
    public static class NotificacionPermissions
    {
        public const string GroupName = "Notification";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(NotificacionPermissions));
        }
    }
}