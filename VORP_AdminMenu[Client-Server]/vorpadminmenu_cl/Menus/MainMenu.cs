using CitizenFX.Core;
using MenuAPI;
using System;
using System.Collections.Generic;
using vorpadminmenu_cl.Functions;

namespace vorpadminmenu_cl.Menus
{
    public class MainMenu
    {
        private static Menu mainMenu = new Menu(GetConfig.Langs["MenuMainTitle"], GetConfig.Langs["MenuMainDesc"]);
        private static bool setupDone = false;
        public static List<object> args = new List<object>();
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(mainMenu);

            string keyPress = GetConfig.Config["key"].ToString();
            int KeyInt = Convert.ToInt32(keyPress, 16);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)KeyInt;

            //Administration
            MenuController.AddSubmenu(mainMenu, AdministrationMenu.GetMenu());

            MenuItem subMenuAdministrationBtn = new MenuItem(GetConfig.Langs["MenuAdministrationTitle"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuAdministrationBtn);
            MenuController.BindMenuItem(mainMenu, AdministrationMenu.GetMenu(), subMenuAdministrationBtn);

            //Boosters
            MenuController.AddSubmenu(mainMenu, BoostersMenu.GetMenu());

            MenuItem subMenuBoostersBtn = new MenuItem(GetConfig.Langs["MenuBoostersTitle"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuBoostersBtn);
            MenuController.BindMenuItem(mainMenu, BoostersMenu.GetMenu(), subMenuBoostersBtn);

            //Notifications
            MenuController.AddSubmenu(mainMenu, NotificationsMenu.GetMenu());

            MenuItem subMenuNotificationsBtn = new MenuItem(GetConfig.Langs["MenuNotificationsTitle"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuNotificationsBtn);
            MenuController.BindMenuItem(mainMenu, NotificationsMenu.GetMenu(), subMenuNotificationsBtn);

            //Teleports
            MenuController.AddSubmenu(mainMenu, TeleportsMenu.GetMenu());

            MenuItem subMenuTeleportsBtn = new MenuItem(GetConfig.Langs["MenuTeleportsTitle"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuTeleportsBtn);
            MenuController.BindMenuItem(mainMenu, TeleportsMenu.GetMenu(), subMenuTeleportsBtn);

            //Database
            MenuController.AddSubmenu(mainMenu, DatabaseMenu.GetMenu());

            MenuItem subMenuDatabaseBtn = new MenuItem(GetConfig.Langs["MenuDatabaseTitle"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuDatabaseBtn);
            MenuController.BindMenuItem(mainMenu, DatabaseMenu.GetMenu(), subMenuDatabaseBtn);

            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == subMenuAdministrationBtn)
                {
                    PlayerFunctions.RequestPlayers();
                    mainMenu.RefreshIndex();
                }
            };
        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }
    }
}
