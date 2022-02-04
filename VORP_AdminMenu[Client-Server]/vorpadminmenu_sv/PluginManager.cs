using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using vorpadminmenu_sv.Diagnostics;
using vorpadminmenu_sv.Scripts;
using static CitizenFX.Core.Native.API;

namespace vorpadminmenu_sv
{
    public class PluginManager : BaseScript
    {
        public static PluginManager Instance { get; private set; }
        public static PlayerList PlayerList;
        public static dynamic CORE;

        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;

        public static Config _scriptConfig = new Config();
 
        public static Dictionary<string, int> ActiveCharacters = new();

        public PluginManager()
        {
            Logger.Info($"Init VORP AdminMenu");

            Instance = this;
            PlayerList = Players;

            Setup();

            Logger.Info($"VORP Admin Menu Loaded");
        }

        public void AttachTickHandler(Func<Task> task)
        {
            Tick += task;
        }

        public void DetachTickHandler(Func<Task> task)
        {
            Tick -= task;
        }

        string _GHMattiMySqlResourceState => GetResourceState("ghmattimysql");

        async Task VendorReady()
        {
            string dbResource = _GHMattiMySqlResourceState;
            if (dbResource == "missing")
            {
                while (true)
                {
                    Logger.Error($"ghmattimysql resource not found! Please make sure you have the resource!");
                    await Delay(1000);
                }
            }

            while (!(dbResource == "started"))
            {
                await Delay(500);
                dbResource = _GHMattiMySqlResourceState;
            }
        }

        async void Setup()
        {
            await VendorReady(); // wait till ghmattimysql resource has started

            GetCore();

            RegisterScript(_scriptConfig);

            AddEvents();
        }

        void GetCore()
        {
            TriggerEvent("getCore", new Action<dynamic>((getCoreResult) =>
            {
                Logger.Success($"VORP Core Setup");
                CORE = getCoreResult;
            }));
        }

        void AddEvents()
        {
            EventRegistry.Add("playerJoined", new Action<Player>(([FromSource] player) =>
            {
                if (!ActiveCharacters.ContainsKey(player.Handle))
                    ActiveCharacters.Add(player.Handle, -1);
            }));

            EventRegistry.Add("playerDropped", new Action<Player, string>(async ([FromSource] player, reason) =>
            {
                try
                {
                    string steamIdent = $"steam:{player.Identifiers["steam"]}";

                    if (ActiveCharacters.ContainsKey(player.Handle))
                        ActiveCharacters.Remove(player.Handle);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"playerDropped: So, they don't exist?!");
                }
            }));

            EventRegistry.Add("onResourceStart", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;

                Logger.Info($"VORP AdminMenu Started");
            }));

            EventRegistry.Add("onResourceStop", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;

                Logger.Info($"Stopping VORP AdminMenu");

                UnregisterScript(_scriptConfig);
            }));
        }
    }
}

