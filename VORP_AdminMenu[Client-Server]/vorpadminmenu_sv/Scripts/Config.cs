using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace vorpadminmenu_sv.Scripts
{
	public class Config : BaseScript
	{
        public static string ConfigString;
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();
        public static string resourcePath = $"{API.GetResourcePath(API.GetCurrentResourceName())}";

        public Config()
		{
			EventHandlers[$"{API.GetCurrentResourceName()}:getConfig"] += new Action<Player>(OnGetConfig);

			SetupConfig();
		}

		private void SetupConfig()
        {
            if (File.Exists($"{resourcePath}/Config.json"))
            {
                ConfigString = File.ReadAllText($"{resourcePath}/Config.json", Encoding.UTF8);
                JObject config = JObject.Parse(ConfigString);
                if (File.Exists($"{resourcePath}/{config["defaultlang"]}.json"))
                {
                    string langstring = File.ReadAllText($"{resourcePath}/{config["defaultlang"]}.json", Encoding.UTF8);
                    Langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(langstring);
                    Debug.WriteLine($"{API.GetCurrentResourceName()}: Language {config["defaultlang"]}.json loaded!");
                }
                else
                {
                    Debug.WriteLine($"{API.GetCurrentResourceName()}: {config["defaultlang"]}.json Not Found");
                }
            }
            else
            {
                Debug.WriteLine($"{API.GetCurrentResourceName()}: Config.json Not Found");
            }
        }

        private void OnGetConfig([FromSource] Player source)
        {
            source.TriggerEvent($"{API.GetCurrentResourceName()}:SendConfig", ConfigString, Langs);
        }
    }
}

