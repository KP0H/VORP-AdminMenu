using System;
using System.Collections.Generic;
using System.Dynamic;
using CitizenFX.Core;
using vorpadminmenu_sv.Diagnostics;

namespace vorpadminmenu_sv.Extensions
{
    static class Common
    {
        public static dynamic GetCoreUser(this Player player)
        {
            if (PluginManager.CORE == null)
            {
                Logger.Error($"GetCoreUser: Core API is null");
                return null;
            }

            return GetCoreUser(int.Parse(player.Handle));
        }

        public static dynamic GetCoreUser(int handle)
        {
            if (PluginManager.CORE == null)
            {
                Logger.Error($"GetCoreUser: Core API is null");
                return null;
            }

            ExpandoObject user = PluginManager.CORE.getUser(handle);

            foreach (var item in user)
            {
                Debug.WriteLine($"{item.Key} {item.Value}");
            }

            return user;
        }

        public static dynamic GetCoreUserCharacter(this Player player)
        {
            dynamic coreUser = player.GetCoreUser();
            if (coreUser == null)
            {
                Logger.Warn($"GetCoreUser: Player '{player.Handle}' does not exist.");
            }
            return coreUser.getUsedCharacter;
        }

        public static dynamic GetCoreUserCharacter(int handle)
        {
            dynamic coreUser = GetCoreUser(handle);
            if (coreUser == null)
            {
                Logger.Warn($"GetCoreUser: Player '{handle}' does not exist.");
            }
            return coreUser.getUsedCharacter;
        }

        public static int GetCoreUserCharacterId(this Player player)
        {
            dynamic character = player.GetCoreUserCharacter();

            if (character == null)
            {
                if (!PluginManager.ActiveCharacters.ContainsKey(player.Handle)) return -1;
                return PluginManager.ActiveCharacters[player.Handle];
            }

            if (!Common.HasProperty(character, "charIdentifier"))
            {
                if (!PluginManager.ActiveCharacters.ContainsKey(player.Handle)) return -1;
                return PluginManager.ActiveCharacters[player.Handle];
            }

            return character?.charIdentifier;
        }

        public static bool HasProperty(ExpandoObject obj, string propertyName)
        {
            return obj != null && ((IDictionary<String, object>)obj).ContainsKey(propertyName);
        }
    }
}

