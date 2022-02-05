﻿using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;
using MenuAPI;
using vorpadminmenu_cl.Functions;

namespace vorpadminmenu_cl.Menus.Players
{

    class PlayersDatabaseMenu
    {
        private static Menu playersListDatabaseMenu = new Menu(GetConfig.Langs["PlayersListTitle"], GetConfig.Langs["PlayersListDesc"]);
        private static Menu playersOptionsDatabaseMenu = new Menu("", GetConfig.Langs["PlayersListDesc"]);
        public static List<int> idPlayers = new List<int>();
        public static int indexPlayer;
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(playersListDatabaseMenu);


            playersListDatabaseMenu.OnMenuOpen += (_menu) =>
            {
                playersListDatabaseMenu.ClearMenuItems();
                idPlayers.Clear();
                foreach (var i in API.GetActivePlayers())
                {
                    string name = API.GetPlayerName(i).ToString();
                    string id = API.GetPlayerServerId(i).ToString();
                    idPlayers.Add(i);
                    MenuController.AddSubmenu(playersListDatabaseMenu, playersOptionsDatabaseMenu);

                    MenuItem playerNameDatabaseButton = new MenuItem(name, $"{name},{id}")
                    {
                        RightIcon = MenuItem.Icon.ARROW_RIGHT
                    };
                    playersListDatabaseMenu.AddMenuItem(playerNameDatabaseButton);
                    MenuController.BindMenuItem(playersListDatabaseMenu, playersOptionsDatabaseMenu, playerNameDatabaseButton);


                }
            };
            playersListDatabaseMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                indexPlayer = _index;
                playersOptionsDatabaseMenu.MenuTitle = API.GetPlayerName(idPlayers.ElementAt(indexPlayer)) + "," + API.GetPlayerServerId((idPlayers.ElementAt(indexPlayer)));

            };

            playersOptionsDatabaseMenu.AddMenuItem(new MenuItem(GetConfig.Langs["AddMoneyTitle"], GetConfig.Langs["AddMoneyDesc"])
            {
                Enabled = true,
            });
            playersOptionsDatabaseMenu.AddMenuItem(new MenuItem(GetConfig.Langs["DelMoneyTitle"], GetConfig.Langs["DelMoneyDesc"])
            {
                Enabled = true,
            });
            playersOptionsDatabaseMenu.AddMenuItem(new MenuItem(GetConfig.Langs["AddXpTitle"], GetConfig.Langs["AddXpDesc"])
            {
                Enabled = true,
            });
            playersOptionsDatabaseMenu.AddMenuItem(new MenuItem(GetConfig.Langs["DelXpTitle"], GetConfig.Langs["DelXpDesc"])
            {
                Enabled = true,
            });
            playersOptionsDatabaseMenu.AddMenuItem(new MenuItem(GetConfig.Langs["AddItemTitle"], GetConfig.Langs["AddItemDesc"])
            {
                Enabled = true,
            });
            playersOptionsDatabaseMenu.AddMenuItem(new MenuItem(GetConfig.Langs["AddWeaponTitle"], GetConfig.Langs["AddWeaponDesc"])
            {
                Enabled = true,
            });

            MenuController.AddSubmenu(playersOptionsDatabaseMenu, Inventory.InventoryMenu.GetMenu());

            MenuItem subMenuInventoryBtn = new MenuItem(GetConfig.Langs["InventoryTitle"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            playersOptionsDatabaseMenu.AddMenuItem(subMenuInventoryBtn);
            MenuController.BindMenuItem(playersOptionsDatabaseMenu, Inventory.InventoryMenu.GetMenu(), subMenuInventoryBtn);


            playersOptionsDatabaseMenu.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_index == 0)
                {
                    MainMenu.args.Add(API.GetPlayerServerId(idPlayers.ElementAt(indexPlayer)));
                    dynamic type = await UtilsFunctions.GetInput(GetConfig.Langs["TypeOfMoneyTitle"], GetConfig.Langs["TypeOfMoneyDesc"]);
                    MainMenu.args.Add(type);
                    dynamic quantity = await UtilsFunctions.GetInput(GetConfig.Langs["Quantity"], GetConfig.Langs["Quantity"]);
                    MainMenu.args.Add(quantity);
                    DatabaseFunctions.AddMoney(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 1)
                {
                    MainMenu.args.Add(API.GetPlayerServerId(idPlayers.ElementAt(indexPlayer)));
                    dynamic type = await UtilsFunctions.GetInput(GetConfig.Langs["TypeOfMoneyTitle"], GetConfig.Langs["TypeOfMoneyDesc"]);
                    MainMenu.args.Add(type);
                    dynamic quantity = await UtilsFunctions.GetInput(GetConfig.Langs["Quantity"], GetConfig.Langs["Quantity"]);
                    MainMenu.args.Add(quantity);
                    DatabaseFunctions.RemoveMoney(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 2)
                {
                    MainMenu.args.Add(API.GetPlayerServerId(idPlayers.ElementAt(indexPlayer)));
                    dynamic quantity = await UtilsFunctions.GetInput(GetConfig.Langs["Quantity"], GetConfig.Langs["Quantity"]);
                    MainMenu.args.Add(quantity);
                    DatabaseFunctions.AddXp(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 3)
                {
                    MainMenu.args.Add(API.GetPlayerServerId(idPlayers.ElementAt(indexPlayer)));
                    dynamic quantity = await UtilsFunctions.GetInput(GetConfig.Langs["Quantity"], GetConfig.Langs["Quantity"]);
                    MainMenu.args.Add(quantity);
                    DatabaseFunctions.RemoveXp(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 4)
                {
                    MainMenu.args.Add(API.GetPlayerServerId(idPlayers.ElementAt(indexPlayer)));
                    dynamic item = await UtilsFunctions.GetInput(GetConfig.Langs["ItemName"], GetConfig.Langs["ItemName"]);
                    MainMenu.args.Add(item);
                    dynamic quantity = await UtilsFunctions.GetInput(GetConfig.Langs["Quantity"], GetConfig.Langs["Quantity"]);
                    MainMenu.args.Add(quantity);
                    DatabaseFunctions.AddItem(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 5)
                {
                    MainMenu.args.Add(API.GetPlayerServerId(idPlayers.ElementAt(indexPlayer)));
                    dynamic weaponName = await UtilsFunctions.GetInput(GetConfig.Langs["WeaponName"], GetConfig.Langs["WeaponName"]);
                    dynamic ammoName = await UtilsFunctions.GetInput(GetConfig.Langs["Weaponammo"], GetConfig.Langs["Weaponammo"]);
                    dynamic ammoQuantity = await UtilsFunctions.GetInput(GetConfig.Langs["Quantity"], GetConfig.Langs["Quantity"]);
                    MainMenu.args.Add(weaponName);
                    MainMenu.args.Add(ammoName);
                    MainMenu.args.Add(ammoQuantity);
                    DatabaseFunctions.AddWeapon(MainMenu.args);
                    MainMenu.args.Clear();
                }
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return playersListDatabaseMenu;
        }
    }
}
