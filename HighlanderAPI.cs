using R2API.Utils;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;
using HighlanderAPI.Modules.ItemTiers;
using HighlanderAPI.Modules.Utils;
using HighlanderAPI.Modules.Artifacts;

namespace HighlanderAPI
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]

    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class HighlanderAPI : BaseUnityPlugin
    {
        public const string ModGuid = "com.BluJay.HighlanderAPI"; //Our Package Name
        public const string ModName = "HighlanderAPI";
        public const string ModVer = "0.0.1";

        internal static BepInEx.Logging.ManualLogSource ModLogger;

        public static ConfigFile HLConfig;
        public static ConfigFile HLBackupConfig;
        public static ConfigEntry<bool> enableAutoConfig { get; set; }
        public static ConfigEntry<string> latestVersion { get; set; }

        public static bool _preVersioning = false;

        public static AssetBundle MainAssets;

        //public List<BuffBase> Buffs = new List<BuffBase>();
        //public List<ItemBase> Items = new List<ItemBase>();
        //public List<EquipmentBase> Equipments = new List<EquipmentBase>();
        public List<ItemTierBase> ItemTiers = new List<ItemTierBase>();
        //public List<EliteEquipmentBase> EliteEquipments = new List<EliteEquipmentBase>();
        public List<ArtifactBase> Artifacts = new List<ArtifactBase>();
        // public List<InteractableBase> Interactables = new List<InteractableBase>();
        // public List<SurvivorBase> Survivors = new List<SurvivorBase>();

        //public static Dictionary<ItemBase, bool> ItemStatusDictionary = new Dictionary<ItemBase, bool>();
        //public static Dictionary<EquipmentBase, bool> EquipmentStatusDictionary = new Dictionary<EquipmentBase, bool>();
        //public static Dictionary<BuffBase, bool> BuffStatusDictionary = new Dictionary<BuffBase, bool>();
        //public static Dictionary<EliteEquipmentBase, bool> EliteEquipmentStatusDictionary = new Dictionary<EliteEquipmentBase, bool>();
        public static Dictionary<ArtifactBase, bool> ArtifactStatusDictionary = new Dictionary<ArtifactBase, bool>();
        //public static Dictionary<InteractableBase, bool> InteractableStatusDictionary = new Dictionary<InteractableBase, bool>();
        // public static Dictionary<SurvivorBase, bool> SurvivorStatusDictionary = new Dictionary<SurvivorBase, bool>();

        public void Awake()
        {
            ModLogger = this.Logger;
            HLConfig = Config;

            HLBackupConfig = new(Paths.ConfigPath + "\\" + ModGuid + "." + ".Backup.cfg", true);
            HLBackupConfig.Bind(": DO NOT MODIFY THIS FILES CONTENTS :", ": DO NOT MODIFY THIS FILES CONTENTS :", ": DO NOT MODIFY THIS FILES CONTENTS :", ": DO NOT MODIFY THIS FILES CONTENTS :");

            enableAutoConfig = HLConfig.Bind("Config", "Enable Auto Config Sync", true, "Disabling this would stop CCElites from syncing config whenever a new version is found.");
            _preVersioning = !((Dictionary<ConfigDefinition, string>)AccessTools.DeclaredPropertyGetter(typeof(ConfigFile), "OrphanedEntries").Invoke(HLConfig, null)).Keys.Any(x => x.Key == "Latest Version");
            latestVersion = HLConfig.Bind("Config", "Latest Version", ModVer, "DO NOT CHANGE THIS");
            if (enableAutoConfig.Value && (_preVersioning || (latestVersion.Value != ModVer)))
            {
                latestVersion.Value = ModVer;
                ConfigManager.VersionChanged = true;
                ModLogger.LogInfo("Config Autosync Enabled.");
            }
        }

        private void Start()
        {

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CCE.assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }
            //Modules.ColorCatalogEntry.Colors.Init();
            //Modules.Utils.ItemTierPickupVFXHelper.SystemInitializer();
            //var disableSurvivor = Config.ActiveBind<bool>("Survivor", "Disable All Survivors?", false, "Do you wish to disable every survivor in Aetherium?");
            /*
            if (true)
            {
                //ItemTier Initialization
                var ColorCatalogEntries = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ColorCatalogEntryBase)));

                ModLogger.LogInfo("-----------------COLORS---------------------");

                foreach (var colorCatalogEntry in ColorCatalogEntries)
                {
                    ColorCatalogEntryBase color = (ColorCatalogEntryBase)System.Activator.CreateInstance(colorCatalogEntry);
                    if (true)//ValidateSurvivor(itemtier, Survivors))
                    {
                        color.Init();

                        ModLogger.LogInfo("Color: " + color.ColorCatalogEntryName + " Initialized!");
                    }
                }
            }
            */
            if (true)
            {
                //ItemTier Initialization
                var ItemTierTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemTierBase)));

                ModLogger.LogInfo("-----------------ITEMTIERS---------------------");

                foreach (var itemTierType in ItemTierTypes)
                {
                    ItemTierBase itemtier = (ItemTierBase)System.Activator.CreateInstance(itemTierType);
                    if (true)
                    {
                        itemtier.Init();

                        ModLogger.LogInfo("ItemTier: " + itemtier.TierName + " Initialized!");
                    }
                }
            }

            //var disableBuffs = Config.Bind<bool>("Buffs", "Disable All Standalone Buffs?", false, "Do you wish to disable every standalone buff in Aetherium?").Value;
            //if (true)
            //{
            //    //Standalone Buff Initialization
            //    var BuffTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BuffBase)));

            //    ModLogger.LogInfo("--------------BUFFS---------------------");

            //    foreach (var buffType in BuffTypes)
            //    {
            //        BuffBase buff = (BuffBase)System.Activator.CreateInstance(buffType);
            //        if (ValidateBuff(buff, Buffs))
            //        {
            //            buff.Init(Config);

            //            ModLogger.LogInfo("Buff: " + buff.BuffName + " Initialized!");
            //        }
            //    }
            //}

            //var disableItems = ConfigManager.ConfigOption<bool>("Items", "Disable All Items?", false, "Do you wish to disable every item in HighlanderAPI?");
            //if (!disableItems)
            //{
            //    //Item Initialization
            //    var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));

            //    ModLogger.LogInfo("----------------------ITEMS--------------------");

            //    foreach (var itemType in ItemTypes)
            //    {
            //        ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
            //        if (!item.BlacklistFromPreLoad && ValidateItem(item, Items))
            //        {
            //            item.Init(Config);

            //            ModLogger.LogInfo("Item: " + item.ItemName + " Initialized!");
            //            //if (item.ItemDef._itemTierDef==Core.instance.itemTierDef)
            //            //{
            //            //    Core.instance.ItemsWithThisTier.Add(item.ItemDef.itemIndex);
            //            //    Core.instance.AvailableTierDropList.Add(PickupCatalog.FindPickupIndex(item.ItemDef.itemIndex));
            //            //    ModLogger.LogWarning("Name" + item.ItemName);
            //            //}
            //            //if (item.ItemDef._itemTierDef== Highlander.instance.itemTierDef)
            //            //{
            //            //    Highlander.instance.ItemsWithThisTier.Add(item.ItemDef.itemIndex);
            //            //    Highlander.instance.AvailableTierDropList.Add(PickupCatalog.FindPickupIndex(item.ItemDef.itemIndex));
            //            //    ModLogger.LogWarning("Name" + item.ItemName);
            //            //}
            //        }
            //    }

            //    //IL.RoR2.ShopTerminalBehavior.GenerateNewPickupServer_bool += ItemBase.BlacklistFromPrinter;
            //    On.RoR2.Items.ContagiousItemManager.Init += ItemBase.RegisterVoidPairings;
            //}
            //var disableEquipment = ConfigManager.ConfigOption<bool>("Equipment", "Disable All Equipment?", false, "Do you wish to disable every equipment in HighlanderAPI?");
            //if (!disableEquipment)
            //{
            //    //Equipment Initialization
            //    var EquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBase)));

            //    ModLogger.LogInfo("-----------------EQUIPMENT---------------------");

            //    foreach (var equipmentType in EquipmentTypes)
            //    {
            //        EquipmentBase equipment = (EquipmentBase)System.Activator.CreateInstance(equipmentType);
            //        if (ValidateEquipment(equipment, Equipments))
            //        {
            //            equipment.Init(Config);

            //            ModLogger.LogInfo("Equipment: " + equipment.EquipmentName + " Initialized!");
            //        }
            //    }
            //}

            //Equipment Initialization
            //var EliteEquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EliteEquipmentBase)));

            //ModLogger.LogInfo("-------------ELITE EQUIPMENT---------------------");

            //foreach (var eliteEquipmentType in EliteEquipmentTypes)
            //{
            //    EliteEquipmentBase eliteEquipment = (EliteEquipmentBase)System.Activator.CreateInstance(eliteEquipmentType);
            //    if (ValidateEliteEquipment(eliteEquipment, EliteEquipments))
            //    {
            //        eliteEquipment.Init(Config);

            //        ModLogger.LogInfo("Elite Equipment: " + eliteEquipment.EliteEquipmentName + " Initialized!");
            //    }
            //}

            ////Artifact Initialization
            //var ArtifactTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ArtifactBase)));

            //ModLogger.LogInfo("-------------ARTIFACTS---------------------");

            //foreach (var ArtifactType in ArtifactTypes)
            //{
            //    ArtifactBase artifact = (ArtifactBase)System.Activator.CreateInstance(ArtifactType);
            //    if (ValidateArtifact(artifact, Artifacts))
            //    {
            //        artifact.Init(Config);

            //        ModLogger.LogInfo("Artifact: " + artifact.ArtifactName + " Initialized!");
            //    }
            //}

            //Compatability
            ModLogger.LogInfo("-------------COMPATIBILITY---------------------");
            ValidateModCompatability();


        }

        //public bool ValidateEliteEquipment(EliteEquipmentBase eliteEquipment, List<EliteEquipmentBase> eliteEquipmentList)
        //{
        //    var enabled = ConfigManager.ConfigOption<bool>("Elite: " + eliteEquipment.EliteModifier, "Enable Elite Equipment?", true, "Should this elite equipment appear in runs? If disabled, the associated elite will not appear in runs either.");

        //    EliteEquipmentStatusDictionary.Add(eliteEquipment, enabled);

        //    if (enabled)
        //    {
        //        eliteEquipmentList.Add(eliteEquipment);
        //        return true;
        //    }
        //    return false;
        //}

        private void ValidateModCompatability()
        {
            var enabledJudgement = ConfigManager.ConfigOption("Mod Compatability: " + "Judgment", "Enable Compatability Patches?", true, "Attempt to add Judgment compatability (if installed)?", true).Value;
            if (ModCompatability.JudgementCompat.IsJudgementInstalled && enabledJudgement)
            {
                ModLogger.LogInfo("ModCompatability: " + "Judgment Recognized!");
                ModCompatability.JudgementCompat.AddJudgementCompat = true;
            }



            //bool IfAnyLoaded = enabledShareSuite || enabledHIV || enabledProperSave;// || enabledEliteReworks;
            //if (IfAnyLoaded)
            //{
            //    ModCompatability.FinishedLoading();
            //}


        }


    }
}