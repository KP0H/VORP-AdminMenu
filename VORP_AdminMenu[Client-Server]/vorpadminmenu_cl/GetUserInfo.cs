﻿using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using vorpadminmenu_cl.Functions.Administration;
using vorpadminmenu_cl.Functions.Boosters;
using vorpadminmenu_cl.Functions.Database;
using vorpadminmenu_cl.Functions.Notifications;
using vorpadminmenu_cl.Functions.Teleports;

namespace vorpadminmenu_cl
{
    class GetUserInfo : BaseScript
    {
        public static string userGroup = "user";
        public static bool loaded = false;
        public GetUserInfo()
        {
            EventHandlers["vorp_admin:GetPlayerInfo"] += new Action<string>(GetPlayerInfo);
            EventHandlers["vorp:SelectedCharacter"] += new Action<int>((charId) => { TriggerServerEvent("vorp_admin:LoadPlayerInfo"); });
        }

        private void GetPlayerInfo(string group)
        {
            userGroup = group;
            SetupMenu();
        }

        private async Task SetupMenu()
        {
            await Delay(2000);
            if (userGroup != "user")
            {
                Menus.MainMenu.GetMenu();
                TeleportsFunctions.SetupTeleports();
                NotificationFunctions.SetupNotifications();
                BoosterFunctions.SetupBoosters();
                AdministrationFunctions.SetupAdministration();
                DatabaseFunctions.SetupDatabase();
                await Delay(2000);
                loaded = true;
            }
        }
    }
}
