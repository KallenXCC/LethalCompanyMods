using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
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
        private const string modVersion = "0.4.1";

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
            harmony.PatchAll();
            mls.LogInfo("patching finished");
        }
    }
}
