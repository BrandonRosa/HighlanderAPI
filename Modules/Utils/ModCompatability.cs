using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using RiskOfOptions;
using RiskOfOptions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace HighlanderAPI.Modules.Utils
{
    internal static class ModCompatability
    {
        public static class RiskOfOptionsCompatability
        {
            public const string GUID = "com.rune580.riskofoptions";
            public static bool IsShareSuiteInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID);

            public static void AddConfig<T>(T config) where T : ConfigEntryBase
            {
                if(config is ConfigEntry<float>)
                {
                    ModSettingsManager.AddOption(new FloatFieldOption(config as ConfigEntry<float>));
                    return;
                }
                if (config is ConfigEntry<bool>)
                {
                    ModSettingsManager.AddOption(new CheckBoxOption(config as ConfigEntry<bool>));
                    return;
                }
                if (config is ConfigEntry<int>)
                {
                    ModSettingsManager.AddOption(new IntFieldOption(config as ConfigEntry<int>));
                    return;
                }
                if (config is ConfigEntry<string>)
                {
                    ModSettingsManager.AddOption(new StringInputFieldOption(config as ConfigEntry<string>));
                    return;
                }
            }
        }

        internal static class HighItemVizabilityCompat
        {
            public static bool IsHighItemVizabilityInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("VizMod.HighItemVizbility");

            public static Hook CustomTiersVizLines;

        }


    }
}
