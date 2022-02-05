using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using vorpadminmenu_cl.Functions;

namespace vorpadminmenu_cl
{
    class VorpAdminMenuClient : BaseScript
    {
        public static bool loaded = false;

        public VorpAdminMenuClient()
        {
            EventHandlers["vorp_adminmenu:InitAdminMenuClient"] += new Action<bool>(InitClient);
            EventHandlers["vorp:SelectedCharacter"] += new Action<int>((charId) => { TriggerServerEvent("vorp_admin:InitAdminMenu"); });
        }

        private void InitClient(bool allowed)
        {
            if (!allowed)
                return;

            SetupMenu();
        }

        private async Task SetupMenu()
        {
            Menus.MainMenu.GetMenu();

            TeleportsFunctions.SetupTeleports();
            NotificationFunctions.SetupNotifications();
            BoosterFunctions.SetupBoosters();
            AdministrationFunctions.SetupAdministration();
            DatabaseFunctions.SetupDatabase();

            await Delay(100);
            loaded = true;
        }
    }
}
