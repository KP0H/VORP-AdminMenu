using System;
using System.Collections.Generic;
using System.Dynamic;
using CitizenFX.Core;

namespace vorpadminmenu_cl.Functions
{
    public class PlayerFunctions : BaseScript
    {
        public static Dictionary<int, string> PlayersList = new Dictionary<int, string>();

        public static void RequestPlayers()
        {
            try
            {
                Debug.WriteLine("Request players");
                TriggerServerEvent("vorp_adminmenu:RequestPlayers");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetPlayers: {ex.Message}");
            }
        }

        [EventHandler("vorp_adminmenu:RecivePlayers")]
        public static void RecivePlayers(ExpandoObject data)
        {
            foreach (var p in data)
            {
                PlayersList.Add(Convert.ToInt32(p.Key), p.Value.ToString());
            }

            foreach (var player in PlayersList)
            {
                Debug.WriteLine($"{player.Key}.{player.Value}");
            }
        }
    }
}

