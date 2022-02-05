using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace vorpadminmenu_cl.Functions
{
	public class PlayerFunctions : BaseScript
	{
		public static async Task<Dictionary<int, string>> GetPlayers()
        {
            try
			{
				Dictionary<int, string> result = new Dictionary<int, string>();

				TriggerServerEvent("vorp_adminmenu:getPlayers", new Action<Dictionary<int, string>>(players => {
					result = players;
				}));

				return result;
			}
			catch (Exception ex)
            {
				Debug.WriteLine($"GetPlayers: {ex.Message}");
            }
			return null;
		}
	}
}

