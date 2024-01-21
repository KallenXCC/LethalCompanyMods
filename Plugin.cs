using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SideQuests.Patches;
using System.Reflection;
using UnityEngine;
using System.IO;
using SideQuests.Managers;

namespace SideQuests
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class SideQuestsBase : BaseUnityPlugin
    {
        private const string modGUID = "KallenXCC.SideQuests";
        private const string modName = "SideQuests";
        private const string modVersion = "0.4.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static SideQuestsBase Instance;

        public GameObject netManagerPrefab;

        internal ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo($"SideQuests loading build version {modVersion}");

            mls.LogInfo("initializing patched NetworkBehaviours...");
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            mls.LogInfo("loading network asset bundle...");
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "netcodemod");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);
            netManagerPrefab = bundle.LoadAsset<GameObject>("Assets/Netcode/SQNetworkManager.prefab");
            netManagerPrefab.AddComponent<SQNetworkManager>();

            mls.LogInfo("harmony patching base...");
            harmony.PatchAll(typeof(SideQuestsBase));
            harmony.PatchAll(typeof(SQNetworkManager));
            harmony.PatchAll(typeof(SQCustomStates));
            mls.LogInfo("patching UI...");
            harmony.PatchAll(typeof(GameNetworkManagerPatch));
            harmony.PatchAll(typeof(StartOfRoundPatch));
            harmony.PatchAll(typeof(HUDManagerPatch));
            harmony.PatchAll(typeof(TerminalPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));
            mls.LogInfo("patching KillQuests...");
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
            mls.LogInfo("patching SecretQuests...");
            harmony.PatchAll(typeof(BlobAIPatch));
            harmony.PatchAll(typeof(PufferAIPatch));
            harmony.PatchAll(typeof(DressGirlAIPatch));
            harmony.PatchAll(typeof(HauntedMaskItemPatch));
            harmony.PatchAll(typeof(TurretPatch));
            harmony.PatchAll(typeof(LandminePatch));
            harmony.PatchAll(typeof(GiftBoxItemPatch));
            harmony.PatchAll(typeof(WhoopieCushionItemPatch));
            harmony.PatchAll(typeof(StormyWeatherPatch));
            mls.LogInfo("patching finished");
            
        }
    }
}
