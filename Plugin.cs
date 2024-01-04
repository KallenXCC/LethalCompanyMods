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
            harmony.PatchAll(typeof(HoarderBugAIPatch));
            harmony.PatchAll(typeof(CrawlerAIPatch));
        }
    }
}
