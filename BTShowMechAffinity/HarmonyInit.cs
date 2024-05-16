using BTShowMechAffinity.Patches;
using Harmony;
using HBS.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BTShowMechAffinity
{
    public static class HarmonyInit
    {
        public static Settings Settings { get; private set; }

        public static void Init(string directory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("io.github.nbk_redspy.BtShowXp");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = JsonConvert.DeserializeObject<Settings>(settingsJSON);
            Settings.Init();

            AffinityHighlight.AffinityColors = Settings.AffinityColors;

            //Logger.Log($"Color Count: {Settings.AffinityColors.Count}");

        }
    }
}
