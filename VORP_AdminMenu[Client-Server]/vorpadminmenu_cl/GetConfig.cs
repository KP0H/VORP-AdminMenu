﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace vorpadminmenu_cl
{
    public class GetConfig : BaseScript
    {
        public static JObject Config = new JObject();
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();

        public GetConfig()
        {
            EventHandlers[$"{API.GetCurrentResourceName()}:SendConfig"] += new Action<string, ExpandoObject>(LoadDefaultConfig);
            TriggerServerEvent($"{API.GetCurrentResourceName()}:getConfig");
        }

        private void LoadDefaultConfig(string dc, ExpandoObject dl)
        {

            Config = JObject.Parse(dc);
            foreach (var l in dl)
            {
                Langs[l.Key] = l.Value.ToString();
            }


            //Start When finish Loading

        }
    }
}
