using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Mre.Sb.Notification.Web.Menus
{
    public class NotificationMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            //Add main menu items.
            context.Menu.AddItem(new ApplicationMenuItem(NotificationMenus.Prefix, displayName: "Notification", "~/Notification", icon: "fa fa-globe"));

            return Task.CompletedTask;
        }
    }
}