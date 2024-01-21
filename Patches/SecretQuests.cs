using GameNetcodeStuff;
using HarmonyLib;

namespace SideQuests.Patches
{
    [HarmonyPatch(typeof(BlobAI))]
    internal class BlobAIPatch
    {
        [HarmonyPatch(nameof(BlobAI.DetectNoise))]
        [HarmonyPostfix]
        static void DetectNoisePatch(ref float ___tamedTimer)
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.SLIME_SID)
            {
                if (___tamedTimer > 0 && !SQCustomStates.taskCompleted)
                {
                    SQCustomStates.CompleteTask();
                }
            }
        }
    }

    [HarmonyPatch(typeof(PufferAI))]
    internal class PufferAIPatch
    {
        [HarmonyPatch(nameof(PufferAI.ShakeTailServerRpc))]
        [HarmonyPostfix]
        static void ShakeTailServerRpcPatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.SPORE_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(DressGirlAI))]
    internal class DressGirlAIPatch
    {
        [HarmonyPatch("BeginChasing")]
        [HarmonyPostfix]
        static void BeginChasingPatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.GHOST_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(HauntedMaskItem))]
    internal class HauntedMaskItemPatch
    {
        [HarmonyPatch("FinishAttaching")]
        [HarmonyPostfix]
        static void FinishAttachingPatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.MASK_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(Turret))]
    internal class TurretPatch
    {
        [HarmonyPatch(nameof(Turret.EnterBerserkModeServerRpc))]
        [HarmonyPostfix]
        static void EnterBerserkModeServerRpcPatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.TURRET_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(Landmine))]
    internal class LandminePatch
    {
        [HarmonyPatch(nameof(Landmine.Detonate))]
        [HarmonyPostfix]
        static void DetonatePatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.LANDMINE_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(GiftBoxItem))]
    internal class GiftBoxItemPatch
    {
        [HarmonyPatch(nameof(GiftBoxItem.OpenGiftBoxServerRpc))]
        [HarmonyPostfix]
        static void OpenGiftBoxServerRpcPatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.GIFT_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(WhoopieCushionItem))]
    internal class WhoopieCushionItemPatch
    {
        [HarmonyPatch(nameof(WhoopieCushionItem.Fart))]
        [HarmonyPostfix]
        static void FartPatch()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.FART_SID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(StormyWeather))]
    internal class StormyWeatherPatch
    {
        private static bool aliveBeforeLightning = false;
        private static bool holdingAKey = false;

        [HarmonyPatch(nameof(StormyWeather.LightningStrike))]
        [HarmonyPrefix]
        static void LightningStrikePatchPre()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.ELECTRICITY_SID)
            {
                PlayerControllerB playerControllerB = GameNetworkManager.Instance.localPlayerController;
                if (!playerControllerB.isPlayerDead)
                {
                    aliveBeforeLightning = true;
                }
                else
                {
                    aliveBeforeLightning = false;
                }
                string heldObjName;
                holdingAKey = false;
                for (int i = 0; i < playerControllerB.ItemSlots.Length; i++)
                {
                    if (playerControllerB.ItemSlots[i] != null)
                    {
                        heldObjName = playerControllerB.ItemSlots[i].itemProperties.itemName;
                        if (heldObjName.Contains("Key"))
                        {
                            holdingAKey = true;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(nameof(StormyWeather.LightningStrike))]
        [HarmonyPostfix]
        static void LightningStrikePatchPost()
        {
            if (SQCustomStates.questID == 2 && SQCustomStates.secretID == SQCustomStates.ELECTRICITY_SID)
            {
                if (aliveBeforeLightning && holdingAKey)
                {
                    PlayerControllerB playerControllerB = GameNetworkManager.Instance.localPlayerController;
                    if (playerControllerB.isPlayerDead)
                    {
                        SQCustomStates.CompleteTask();
                    }
                }
            }
        }
    }
}
