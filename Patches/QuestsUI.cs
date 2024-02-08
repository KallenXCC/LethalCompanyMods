using HarmonyLib;
using SideQuests.Managers;
using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace SideQuests.Patches
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void AddToPrefabs(ref GameNetworkManager __instance)
        {
            __instance.GetComponent<NetworkManager>().AddNetworkPrefab(SideQuestsBase.Instance.netManagerPrefab);
            //Console.WriteLine("Added SQNetworkManager Prefab");
        }
    }

    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void SpawnSQNetworkManager(StartOfRound __instance)
        {
            if(__instance.IsHost)
            {
                GameObject go = GameObject.Instantiate(SideQuestsBase.Instance.netManagerPrefab);
                go.GetComponent<NetworkObject>().Spawn();
                //Console.WriteLine("Spawned SQNetworkManager");
            }
        }
    }

    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        [HarmonyPatch(nameof(HUDManager.ClearControlTips))]
        [HarmonyPostfix]
        static void ClearControlTipsPatch(ref TextMeshProUGUI[] ___controlTipLines)
        {
            if (SQCustomStates.taskCompleted)
            {
                ___controlTipLines[0].text = "Task Completed!\nUse Terminal to submit";
            }
            else
            {
                ___controlTipLines[0].text = SQCustomStates.questDesc[SQCustomStates.questID];
            }
        }
    }

    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        static IEnumerator ChangeControlTipMultipleDelayed(string[] allLines)
        {
            // Wait for a short duration to ensure the original coroutine finishes
            yield return new WaitForSeconds(0.1f);

            HUDManager.Instance.ChangeControlTipMultiple(allLines, holdingItem: false, itemProperties: null);
        }

        [HarmonyPatch(nameof(Terminal.BeginUsingTerminal))]
        [HarmonyPostfix]
        static void BeginUsingTerminalPatch()
        {
            string[] allLines = new string[3];
            if (StartOfRound.Instance.localPlayerUsingController)
            {
                allLines[0] = "Quit Terminal : [Start]";
            }
            else
            {
                allLines[0] = "Quit Terminal : [TAB]";
            }
            if(SQCustomStates.taskCompleted)
            {
                allLines[1] = "Task Completed!";
                allLines[2] = "\nEnter 'submit' in terminal\nto complete quest";
            }
            else
            {
                allLines[1] = SQCustomStates.questDesc[SQCustomStates.questID];
                allLines[2] = "\nEnter 'abandon' in terminal\nto abandon quest";
            }
            ((MonoBehaviour)HUDManager.Instance).StartCoroutine(ChangeControlTipMultipleDelayed(allLines));
        }

        [HarmonyPatch(nameof(Terminal.OnSubmit))]
        [HarmonyPrefix]
        static void OnSubmitPatch(ref TMP_InputField ___screenText, Terminal __instance)
        {
            if (___screenText.text.Length >= 7)
            {
                //Console.WriteLine(___screenText.text);
                if (___screenText.text.Contains("abandon"))
                {
                    SQCustomStates.AbandonQuest();
                    // ___screenText.text = "Quest Abandoned";
                }
                if (___screenText.text.Contains("submit"))
                {
                    if (SQCustomStates.taskCompleted)
                    {
                        HUDManager.Instance.DisplayTip("Quest Complete!", SQCustomStates.questDesc[SQCustomStates.questID]);
                        int reward = SQCustomStates.GetReward();
                        int newGroupCredits = __instance.groupCredits + reward;
                        GrabbableObject[] questReward = new GrabbableObject[0];
                        int quotaFulfilled = TimeOfDay.Instance.quotaFulfilled + reward;
                        // int totalScrapValue = StartOfRound.Instance.gameStats.scrapValueCollected + reward;

                        __instance.groupCredits = newGroupCredits;
                        HUDManager.Instance.DisplayCreditsEarning(reward, questReward, newGroupCredits);
                        if (__instance.IsHost || __instance.IsServer)
                        {
                            //Console.WriteLine("Host/Server SQN request");
                            SQNetworkManager.Instance.SyncQuotaFulfilledClientRpc(quotaFulfilled);
                            //Console.WriteLine("Host/Server SQN request creditSync");
                            SQNetworkManager.Instance.SyncGroupCreditsClientRpc(newGroupCredits);
                        }
                        else
                        {
                            //Console.WriteLine("Client SQN request quotaSync");
                            SQNetworkManager.Instance.SyncQuotaFulfilledServerRpc(quotaFulfilled);
                            //Console.WriteLine("Client SQN request creditSync");
                            SQNetworkManager.Instance.SyncGroupCreditsServerRpc(newGroupCredits);
                        }

                        SQCustomStates.RandomizeQuest();
                        // ___screenText.text = "Quest Completed";
                    } else
                    {
                        // ___screenText.text = "Task Incomplete";
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        [HarmonyPatch(nameof(RoundManager.CollectNewScrapForThisRound))]
        [HarmonyPostfix]
        static void CollectNewScrapForThisRoundPatch(ref GrabbableObject scrapObject)
        {
            if (SQCustomStates.questID == 0)
            {
                string itemCollected = scrapObject.itemProperties.itemName.Substring(0, 3);
                string questItem = SQCustomStates.GetItemName().Substring(0, 3);
                if (string.Compare(itemCollected, questItem, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SQCustomStates.CompleteTask();
                }
            }
        }
    }
}
