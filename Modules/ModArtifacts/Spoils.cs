using BepInEx.Configuration;
using HighlanderAPI.Modules.ItemTiers.HighlanderTier;
using HighlanderAPI.Modules.Utils;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using RoR2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static HighlanderAPI.HighlanderAPI;
using static HighlanderAPI.Modules.Utils.ItemHelpers;

namespace HighlanderAPI.Modules.Artifacts
{
    class Spoils : ArtifactBase<Spoils>
    {
        public override string ArtifactName => "Artifact of Spoils";
        public override string ArtifactLangTokenName => "SPOILS";
        public override string ArtifactFullDescription => $"Highlanders will always drop as void potentials.";

        public override GameObject ArtifactModel => MainAssets.LoadAsset<GameObject>("Assets/Textrures/Icons/Temporary/crystal3/source/Ferocity.prefab");
        public override Sprite ArtifactIconDeselected => MainAssets.LoadAsset<Sprite>("Assets/Models/ArtifactOfSpoils/SpoilsDisable.png");
        public override Sprite ArtifactIconSelected => MainAssets.LoadAsset<Sprite>("Assets/Models/ArtifactOfSpoils/SpoilsEnabled.png");

        public static GameObject potentialPrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_OptionPickup.OptionPickup_prefab)).WaitForCompletion();

        public static ConfigEntry<int> AdditionalChoices;

        public override void Init(ConfigFile config)
        {

            CreateConfig(config);
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            AdditionalChoices = ConfigManager.ConfigOption<int>("Artifact: " +ArtifactName, "Extra choices", 2, "How many additional choices should you give players with Artifact of Spoils?");
        }

        public override void Hooks()
        {

        }


        public static GenericPickupController.CreatePickupInfo SpoilsPickupInfo(Transform position)
        {
            return SpoilsPickupInfo(1 + AdditionalChoices.Value, position);
        }

        public static GenericPickupController.CreatePickupInfo SpoilsPickupInfo(Vector3 position)
        {
            return SpoilsPickupInfo(1 + AdditionalChoices.Value, position);
        }
        public static GenericPickupController.CreatePickupInfo SpoilsPickupInfo(int count, Transform position)
        {
            return SpoilsPickupInfo(count, position.position);
        }
        public static GenericPickupController.CreatePickupInfo SpoilsPickupInfo(int count, Vector3 position)
        {
            List<PickupIndex> Highlist=ItemHelpers.PickupDefsWithTier(Highlander.instance.itemTierDef).Select(x => x.pickupIndex).ToList(); ;
            PickupIndex[] pickupIndex = ItemHelpers.GetRandomSelectionFromArray(Highlist, count, RoR2Application.rng);

            GenericPickupController.CreatePickupInfo PickupInfo = new GenericPickupController.CreatePickupInfo
            {
                pickerOptions = PickupPickerController.GenerateOptionsFromArray(pickupIndex),
                prefabOverride = potentialPrefab,
                position = position,
                rotation = Quaternion.identity,
                pickupIndex = PickupCatalog.FindPickupIndex(Highlander.instance.itemTierDef._tier)
            };
            PickupInfo.prefabOverride = potentialPrefab;
            return PickupInfo;
        }
    }
}
