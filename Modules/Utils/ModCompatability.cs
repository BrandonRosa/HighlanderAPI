using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
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

            public static void AddConfig<T>(T config, bool requiresRestart) where T : ConfigEntryBase
            {
                BaseOption option = null;
                if (config is ConfigEntry<float>)
                {
                    option = new FloatFieldOption(config as ConfigEntry<float>, requiresRestart);
                }
                else if (config is ConfigEntry<bool>)
                {
                    option = new CheckBoxOption(config as ConfigEntry<bool>, requiresRestart);
                }
                else if (config is ConfigEntry<int>)
                {
                    option = new IntFieldOption(config as ConfigEntry<int>, requiresRestart);
                }
                else if (config is ConfigEntry<string>)
                {
                    option = new StringInputFieldOption(config as ConfigEntry<string>, requiresRestart);
                }
                else
                {
                    option = new RiskOfOptions.Options.ChoiceOption(config, requiresRestart);
                }
                ModSettingsManager.AddOption(option);
                return;
            }
        }

        internal static class JudgementCompat
        {
            public static bool IsJudgementInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Nuxlar.Judgement");

            public static bool AddJudgementCompat = true;

            public static bool IsInJudgementRun() => IsJudgementInstalled && AddJudgementCompat && Run.instance.gameModeIndex == GameModeCatalog.FindGameModeIndex("xJudgementRun");


        }

        internal static class HighItemVizabilityCompat
        {
            public static bool IsHighItemVizabilityInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("VizMod.HighItemVizbility");

            public static Hook CustomTiersVizLines;

        }


    }
}
