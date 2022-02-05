using System.Collections.Generic;
using System.Linq;
using MenuAPI;
using vorpadminmenu_cl.Functions;

namespace vorpadminmenu_cl.Menus.Players
{
    class PlayersMenu
    {
        private static Menu playersListMenu = new Menu(GetConfig.Langs["PlayersListTitle"], GetConfig.Langs["PlayersListDesc"]);
        private static Menu playersOptionsMenu = new Menu("", GetConfig.Langs["PlayersListDesc"]);
        private static List<KeyValuePair<int, string>> idPlayers = new List<KeyValuePair<int, string>>();

        private static int indexPlayer;
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(playersListMenu);

            playersListMenu.OnMenuOpen += (_menu) =>
            {
                playersListMenu.ClearMenuItems();
                idPlayers.Clear();

                foreach (KeyValuePair<int, string> player in PlayerFunctions.PlayersList)
                {
                    idPlayers.Add(player);

                    MenuController.AddSubmenu(playersListMenu, playersOptionsMenu);
                    MenuItem playerNameButton = new MenuItem(player.Value, $"{player.Value},{player.Key}")
                    {
                        RightIcon = MenuItem.Icon.ARROW_RIGHT
                    };
                    playersListMenu.AddMenuItem(playerNameButton);
                    MenuController.BindMenuItem(playersListMenu, playersOptionsMenu, playerNameButton);
                }
            };

            playersListMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                indexPlayer = _index;
                playersOptionsMenu.MenuTitle = idPlayers.ElementAt(indexPlayer).Value + "," + idPlayers.ElementAt(indexPlayer).Key;
            };

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["SpectateTitle"], GetConfig.Langs["SpectateDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["SpectateTitleOff"], GetConfig.Langs["SpectateDescOff"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["ReviveTitle"], GetConfig.Langs["ReviveDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["HealTitle"], GetConfig.Langs["HealDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["TpToPlayerTitle"], GetConfig.Langs["TpToPlayerDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["BringPlayerTitle"], GetConfig.Langs["BringPlayerDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["FreezeTitle"], GetConfig.Langs["FreezeDesc"])
            {
                Enabled = true,
            });
            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["KickPlayerTitle"], GetConfig.Langs["KickPlayerDesc"])
            {
                Enabled = true,
            });
            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["BanPlayerTitle"], GetConfig.Langs["BanPlayerDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["SlapTitle"], GetConfig.Langs["SlapDesc"])
            {
                Enabled = true,
            });
            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["LightningTitle"], GetConfig.Langs["LightningDesc"])
            {
                Enabled = true,
            });
            playersOptionsMenu.AddMenuItem(new MenuItem(GetConfig.Langs["FireTitle"], GetConfig.Langs["FireDesc"])
            {
                Enabled = true,
            });

            playersOptionsMenu.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_index == 0)//Spectate
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.Spectate(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 1)//Spectate off
                {
                    AdministrationFunctions.SpectateOff(MainMenu.args);
                }
                else if (_index == 2)//Revive
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.Revive(MainMenu.args);
                }
                else if (_index == 3)//Heal
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.Heal(MainMenu.args);
                }
                else if (_index == 4)//TpToPlayer
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    TeleportsFunctions.TpToPlayer(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 5)//TpBring
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    TeleportsFunctions.TpBring(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 6)//Stop Player
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.StopPlayer(MainMenu.args);
                    MainMenu.args.Clear();
                }

                else if (_index == 7)//Kick
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.Kick(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 8)//Ban
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    dynamic time = await UtilsFunctions.GetInput(GetConfig.Langs["BanPlayerTitle"], GetConfig.Langs["BanPlayerTime"]);
                    MainMenu.args.Add(time);
                    dynamic reason = await UtilsFunctions.GetInput(GetConfig.Langs["BanPlayerTitle"], GetConfig.Langs["BanPlayerReason"]);
                    MainMenu.args.Add(reason);
                    AdministrationFunctions.Ban(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 9)//Slap
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.Slap(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 10)//ThorTold
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.ThorToId(MainMenu.args);
                    MainMenu.args.Clear();
                }
                else if (_index == 11)//FireTiId
                {
                    MainMenu.args.Add(idPlayers.ElementAt(indexPlayer).Key);
                    AdministrationFunctions.FireToId(MainMenu.args);
                    MainMenu.args.Clear();
                }
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return playersListMenu;
        }
    }
}
