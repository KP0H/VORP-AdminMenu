using CitizenFX.Core;
using System;
using System.Collections.Generic;
using vorpadminmenu_sv.Diagnostics;
using vorpadminmenu_sv.Extensions;

namespace vorpadminmenu_sv
{
    class VorpAdminMenuServer : BaseScript
    {
        PlayerList PlayersList;

        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;

        public VorpAdminMenuServer()
        {
            PlayersList = Players;

            AddEvents();
        }

        private void AddEvents()
        {
            EventRegistry.Add("vorp_admin:InitAdminMenu", InitAdminMenu);

            EventRegistry.Add("vorp_adminmenu:getPlayers", GetPlayers);

            #region client-server events
            EventRegistry.Add("vorp:ownerCoordsToBring", CoordsToBringPlayer);

            EventHandlers["vorp:ownerCoordsToBring"] += new Action<Vector3, int>(CoordsToBringPlayer);
            EventHandlers["vorp:askCoordsToTPPlayerDestiny"] += new Action<Player, int>(CoordsToPlayerDestiny);
            EventHandlers["vorp:callbackCoords"] += new Action<string, Vector3>(CoordsToStart);

            EventHandlers["vorp:privateMessage"] += new Action<Player, int, string>(PrivateMessage);
            EventHandlers["vorp:broadCastMessage"] += new Action<Player, string>(BroadCastMessage);

            EventHandlers["vorp:thor"] += new Action<Vector3>(ThorServer);

            EventHandlers["vorp:kick"] += new Action<Player, int>(Kick);
            EventHandlers["vorp:slap"] += new Action<Player, int>(Slap);
            EventHandlers["vorp:stopplayer"] += new Action<Player, int>(StopP);

            EventHandlers["vorp:thorIDserver"] += new Action<Player, int>(ThorToId);
            EventHandlers["vorp:fireIDserver"] += new Action<Player, int>(FireToId);

            EventHandlers["vorp:revivePlayer"] += new Action<Player, int>(RevivePlayer);
            EventHandlers["vorp:healPlayer"] += new Action<Player, int>(HealPlayer);
            #endregion

            #region database events

            EventHandlers["vorp:adminAddMoney"] += new Action<Player, List<object>>(AdminAddMoney);
            EventHandlers["vorp:adminRemoveMoney"] += new Action<Player, List<object>>(AdminRemoveMoney);
            EventHandlers["vorp:adminAddXp"] += new Action<Player, List<object>>(AdminAddXp);
            EventHandlers["vorp:adminRemoveXp"] += new Action<Player, List<object>>(AdminRemoveXp);

            EventHandlers["vorp:adminAddItem"] += new Action<Player, List<object>>(AdminAddItem);
            EventHandlers["vorp:adminDelItem"] += new Action<Player, List<object>>(AdminDelItem);
            EventHandlers["vorp:adminAddWeapon"] += new Action<Player, List<object>>(AdminAddWeapon);

            EventHandlers["vorp:getInventory"] += new Action<Player, List<object>>(GetInventory);

            #endregion
        }

        private void InitAdminMenu([FromSource] Player source)
        {
            dynamic UserCharacter = Common.GetCoreUserCharacter(source);

            source.TriggerEvent("vorp_adminmenu:InitAdminMenuClient", UserCharacter.group == "admin");
        }

        public void GetPlayers([FromSource] Player source, NetworkCallbackDelegate callback)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var player in PluginManager.PlayerList)
            {
                var character = source.GetCoreUserCharacter();
                if (character == null)
                {
                    continue;
                }

                result.Add(int.Parse(source.Handle), $"{character.firstname} {character.lastname}");
            }
            callback.Invoke(result);
        }

        #region client-server events

        private void CoordsToBringPlayer(Vector3 coordToSend, int destinataryID)
        {
            try
            {
                Player p = PlayersList[destinataryID];
                TriggerClientEvent(p, "vorp:sendCoordsToDestinyBring", coordToSend);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"CoordsToBringPlayer");
            }
        }

        private void CoordsToPlayerDestiny([FromSource] Player ply, int destinataryID)
        {
            try
            {
                Player p = PlayersList[destinataryID];
                TriggerClientEvent(p, "vorp:askForCoords", ply.Handle);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"CoordsToPlayerDestiny");
            }
        }

        private void CoordsToStart(string sourceID, Vector3 coordsDestiny)
        {
            try
            {
                Player p = PlayersList[int.Parse(sourceID)];
                TriggerClientEvent(p, "vorp:coordsToStart", coordsDestiny);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"CoordsToStart");
            }
        }

        private void PrivateMessage([FromSource] Player player, int id, string message)
        {
            try
            {
                Player p = PlayersList[id];
                TriggerClientEvent(p, "vorp:Tip", message, 8000);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"PrivateMessage");
            }
        }

        private void BroadCastMessage([FromSource] Player player, string message)
        {
            TriggerClientEvent("vorp:NotifyLeft", player.Name, message, "generic_textures", "tick", 12000);
        }


        private void ThorServer(Vector3 thorCoords)
        {
            TriggerClientEvent("vorp:thordone", thorCoords);
        }

        private void StopP([FromSource] Player player, int id)
        {
            try
            {
                Player p = PlayersList[id];
                TriggerClientEvent(p, "vorp:stopit");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"StopP");
            }
        }

        private void Slap([FromSource] Player player, int idDestinatary)
        {
            try
            {
                Player p = PlayersList[idDestinatary];
                p.TriggerEvent("vorp:slapback");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Slap");
            }
        }

        private void Kick([FromSource] Player player, int id)
        {
            try
            {
                Player p = PlayersList[id];
                p.Drop("Kicked by Staff");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Kick");
            }
        }

        private void ThorToId([FromSource] Player player, int idDestinatary)
        {
            try
            {
                Player p = PlayersList[idDestinatary];
                TriggerClientEvent(p, "vorp:thorIDdone");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"ThorToId");
            }
        }

        private void FireToId([FromSource] Player player, int idDestinatary)
        {
            try
            {
                Player p = PlayersList[idDestinatary];
                TriggerClientEvent(p, "vorp:fireIDdone");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"FireToId");
            }
        }

        private void RevivePlayer([FromSource] Player player, int idDestinatary)
        {
            try
            {
                if (idDestinatary != -1)
                {
                    Player p = PlayersList[idDestinatary];
                    TriggerClientEvent(p, "vorp:resurrectPlayer");
                }
                else
                {
                    player.TriggerEvent("vorp:resurrectPlayer");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"RevivePlayer");
            }
        }

        private void HealPlayer([FromSource] Player player, int idDestinatary)
        {
            try
            {
                if (idDestinatary != -1)
                {
                    Player p = PlayersList[idDestinatary];
                    p.TriggerEvent("vorpmetabolism:setValue", "Thirst", 1000);
                    p.TriggerEvent("vorpmetabolism:setValue", "Hunger", 1000);
                    p.TriggerEvent("vorp:healDone");
                }
                else
                {
                    player.TriggerEvent("vorpmetabolism:setValue", "Thirst", 1000);
                    player.TriggerEvent("vorpmetabolism:setValue", "Hunger", 1000);
                    player.TriggerEvent("vorp:healDone");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"HealPlayer");
            }
        }

        #endregion

        #region database events

        private void AdminAddMoney([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            bool typeC = int.TryParse(args[1].ToString(), out int type);

            dynamic UserCharacter = Common.GetCoreUserCharacter(id);

            if (idC && typeC)
            {
                if (type == 2)
                {
                    bool quantityC = double.TryParse(args[2].ToString(), out double quantity);
                    if (quantityC)
                    {
                        int intQuantity = (int)Math.Ceiling(quantity);
                        UserCharacter.addCurrency(type, intQuantity);
                    }
                    else
                    {
                        bool quantityCInt = int.TryParse(args[2].ToString(), out int quantityInt);
                        if (quantityCInt)
                        {
                            UserCharacter.addCurrency(type, quantityInt);
                        }
                        else
                        {
                            Debug.WriteLine("Bad syntax");
                        }
                    }
                }
                else if (type == 0 || type == 1)
                {
                    bool quantityC = double.TryParse(args[2].ToString(), out double quantity);
                    if (quantityC)
                    {
                        UserCharacter.addCurrency(type, quantity);
                    }
                }
                else
                {
                    Debug.WriteLine("Bad syntax");
                }
            }
            else
            {
                Debug.WriteLine("Bad syntax");
            }
        }

        private void AdminRemoveMoney([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            bool typeC = int.TryParse(args[1].ToString(), out int type);

            dynamic UserCharacter = Common.GetCoreUserCharacter(id);

            if (idC && typeC)
            {
                if (type == 2)
                {
                    bool quantityC = double.TryParse(args[2].ToString(), out double quantity);
                    if (quantityC)
                    {
                        int intQuantity = (int)Math.Ceiling(quantity);
                        UserCharacter.removeCurrency(type, intQuantity);
                    }
                    else
                    {
                        bool quantityCInt = int.TryParse(args[2].ToString(), out int quantityInt);
                        if (quantityCInt)
                        {
                            UserCharacter.removeCurrency(type, quantityInt);
                        }
                        else
                        {
                            Debug.WriteLine("Bad syntax");
                        }
                    }
                }
                else if (type == 0 || type == 1)
                {
                    bool quantityC = double.TryParse(args[2].ToString(), out double quantity);
                    if (quantityC)
                    {
                        UserCharacter.removeCurrency(type, quantity);
                    }
                }
                else
                {
                    Logger.Warn("Bad syntax");
                }
            }
            else
            {
                Logger.Warn("Bad syntax");
            }
        }

        private void AdminAddXp([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            bool quantityC = int.TryParse(args[1].ToString(), out int quantity);
            dynamic UserCharacter = Common.GetCoreUserCharacter(id);
            if (idC && quantityC)
            {
                UserCharacter.addXp(quantity);
            }
            else
            {
                Debug.WriteLine("Bad syntax");
            }
        }

        private void AdminRemoveXp([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            bool quantityC = int.TryParse(args[1].ToString(), out int quantity);
            dynamic UserCharacter = Common.GetCoreUserCharacter(id);
            if (idC && quantityC)
            {
                UserCharacter.removeXp(quantity);
            }
            else
            {
                Debug.WriteLine("Bad syntax");
            }
        }

        private void AdminAddItem([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            string item = args[1].ToString();
            bool quantityC = int.TryParse(args[2].ToString(), out int quantity);


            if (idC && quantityC)
            {
                Exports["ghmattimysql"].execute("SELECT * FROM items WHERE item=(?)", new[] { item }, new Action<dynamic>((result) =>
                {
                    if (result.Count != 0)
                    {
                        TriggerEvent("vorpCore:addItem", id, item, quantity);
                    }
                    else
                    {
                        Debug.WriteLine(item + " doesn't exist in db");
                    }

                }));

            }
            else
            {
                Debug.WriteLine("Bad syntax");
            }
        }

        private void AdminDelItem([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            string item = args[1].ToString();
            bool quantityC = int.TryParse(args[2].ToString(), out int quantity);
            if (idC && quantityC)
            {
                TriggerEvent("vorpCore:subItem", id, item, quantity);
            }
            else
            {
                Debug.WriteLine("Bad syntax");
            }
        }

        private void AdminAddWeapon([FromSource] Player source, List<object> args)
        {
            bool idC = int.TryParse(args[0].ToString(), out int id);
            string item = args[1].ToString();
            bool wC = false;
            foreach (string w in Utils.WeaponList.weapons)
            {
                if (w == item)
                {
                    wC = true;
                    Debug.WriteLine("Va bien");
                }
            }

            string ammo = args[2].ToString();
            bool aC = false;
            foreach (string a in Utils.AmmoList.ammo)
            {
                if (a == ammo)
                {
                    aC = true;
                    Debug.WriteLine("Esta bien");
                }
            }
            bool quantityC = int.TryParse(args[3].ToString(), out int quantity);
            if (idC && wC && aC && quantityC)
            {
                Debug.WriteLine("No entiendo nada");
                Dictionary<string, int> ammoaux = new Dictionary<string, int>();
                ammoaux.Add(ammo, quantity);
                TriggerEvent("vorpCore:registerWeapon", id, item, ammoaux, ammoaux);
            }
            else
            {
                Debug.WriteLine("Bad syntax");
            }
        }

        private void GetInventory([FromSource] Player source, List<object> args)
        {
            int idPlayer = int.Parse(args[0].ToString());
            TriggerEvent("vorpCore:getUserInventory", idPlayer, new Action<dynamic>((items) =>
            {
                source.TriggerEvent("vorp:loadPlayerInventory", items);
            }));
        }

        #endregion
    }
}
