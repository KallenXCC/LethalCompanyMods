using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SideQuests.Patches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SideQuests
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class SideQuestsBase : BaseUnityPlugin
    {
        private const string modGUID = "KallenXCC.SideQuests";
        private const string modName = "Side Quests";
        private const string modVersion = "0.0.0.3";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static SideQuestsBase Instance;

        internal ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo($"SideQuests loading build version {modVersion}");

            harmony.PatchAll(typeof(SideQuestsBase));
            harmony.PatchAll(typeof(PatchCustomStates));
            harmony.PatchAll(typeof(HUDManagerPatch));
            harmony.PatchAll(typeof(TerminalPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(DoublewingAIPatch));
            harmony.PatchAll(typeof(HoarderBugAIPatch));
            harmony.PatchAll(typeof(CentipedeAIPatch));
            harmony.PatchAll(typeof(CrawlerAIPatch));
            harmony.PatchAll(typeof(SandSpiderAIPatch));
            harmony.PatchAll(typeof(MaskedPlayerEnemyPatch));
            harmony.PatchAll(typeof(BaboonBirdAIPatch));
            harmony.PatchAll(typeof(NutcrackerEnemyAIPatch));
            harmony.PatchAll(typeof(FlowermanAIPatch));
            harmony.PatchAll(typeof(MouthDogAIPatch));
            harmony.PatchAll(typeof(ForestGiantAIPatch));
            harmony.PatchAll(typeof(BlobAIPatch));
            harmony.PatchAll(typeof(PufferAIPatch));
            harmony.PatchAll(typeof(DressGirlAIPatch));
            harmony.PatchAll(typeof(HauntedMaskItemPatch));
            harmony.PatchAll(typeof(TurretPatch));
            harmony.PatchAll(typeof(LandminePatch));
            harmony.PatchAll(typeof(GiftBoxItemPatch));
            harmony.PatchAll(typeof(WhoopieCushionItemPatch));
            harmony.PatchAll(typeof(StormyWeatherPatch));
        }
    }
}
