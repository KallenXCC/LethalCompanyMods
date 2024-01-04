using GameNetcodeStuff;
using HarmonyLib;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

namespace SideQuests.Patches
{
    public static class PatchCustomStates
    {
        public static string[] questDesc = new string[3];
        public static int questNum = 0;
        public static int itemNum = 0;
        public static int enemyNum = 0;
        public static bool taskCompleted = false;

        public static string turnInQuest = "Task Completed! \nUse Terminal to Complete";
        public static string questCompleteText = "Quest Completed!";
        readonly static string[] itemList = new string[46];

        static System.Random random = new System.Random();

        static PatchCustomStates()
        {
            RandomizeQuest();

            itemList[0] = "Gift Box";
            itemList[1] = "Big bolt";
            itemList[2] = "V-type engine";
            itemList[3] = "Large axle";
            itemList[4] = "Metal sheet";
            itemList[5] = "Bottles";
            itemList[6] = "Tea kettle";
            itemList[7] = "Brass bell";
            itemList[8] = "Plastic fish";
            itemList[9] = "Flask";
            itemList[10] = "Toy cube";
            itemList[11] = "Magnifying glass";
            itemList[12] = "Hair brush";
            itemList[13] = "Stop sign";
            itemList[14] = "Robot toy";
            itemList[15] = "Cookie mold pan";
            itemList[16] = "Dust pan";
            itemList[17] = "Clown horn";
            itemList[18] = "Coffee mug";
            itemList[19] = "Tragedy";
            itemList[20] = "Airhorn";
            itemList[21] = "Comedy";
            itemList[22] = "Fancy lamp";
            itemList[23] = "Red soda";
            itemList[24] = "Egg beater";
            itemList[25] = "Whoopie-Cushion";
            itemList[26] = "DIY-Flashbang";
            itemList[27] = "Teeth";
            itemList[28] = "Toothpaste";
            itemList[29] = "Old phone";
            itemList[30] = "Jar of pickles";
            itemList[31] = "Rubber Ducky";
            itemList[32] = "Remote";
            itemList[33] = "Steering wheel";
            itemList[34] = "Golden cup";
            itemList[35] = "Yield sign";
            itemList[36] = "Perfume bottle";
            itemList[37] = "Ring";
            itemList[38] = "Laser pointer";
            itemList[39] = "Chemical jug";
            itemList[40] = "Painting";
            itemList[41] = "Cash register";
            itemList[42] = "Candy";
            itemList[43] = "Pill bottle";
            itemList[44] = "Apparatus";
            itemList[45] = "Bee Hive";

            questDesc[0] = "Quest: \nCollect " + itemList[itemNum];
            questDesc[1] = "Quest: \nKill a Loot Bug";
            questDesc[2] = "Quest: \nKill a Thumper";


        }

        public static void RandomizeQuest()
        {
            taskCompleted = false;
            int randomNumber = random.Next(0, 10);
            if (randomNumber < 7) // item quest 70%
            {
                questNum = 0;
                randomNumber = random.Next(0, 10);
                if (randomNumber < 6) // easy item 60%
                {
                    randomNumber = random.Next(0, 10);
                } else if (randomNumber < 9) // medium item 30%
                {
                    randomNumber = random.Next(10, 27);
                } else // hard item 10%
                {
                    randomNumber = random.Next(27, 46);
                }
                itemNum = randomNumber;
                questDesc[0] = "Quest: \nCollect " + itemList[itemNum];
            } else if (randomNumber < 9) // kill quest 20%
            {
                questNum = 1;
            } else // special quest 10%
            {
                questNum = 2;
            }
        }

        public static void CompleteTask()
        {
            taskCompleted = true;
            HUDManager.Instance.ClearControlTips();
        }

        public static string GetItemName()
        {
            return itemList[itemNum];
        }

        public static int GetReward()
        {
            int reward = 50 + questNum * 50;
            if (questNum == 0)
            {
                reward += itemNum * 3;
            } else if (questNum == 1)
            {
                reward += enemyNum * 25;
            }
            return reward;
        }

        public static void AbandonQuest()
        {
            RandomizeQuest();
            HUDManager.Instance.ClearControlTips();
        }
    }


    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        

        [HarmonyPatch(nameof(HUDManager.ClearControlTips))]
        [HarmonyPostfix]
        static void ClearControlTipsPatch(ref TextMeshProUGUI[] ___controlTipLines)
        {
            if (PatchCustomStates.taskCompleted)
            {
                ___controlTipLines[0].text = PatchCustomStates.turnInQuest;
            }
            else
            {
                ___controlTipLines[0].text = PatchCustomStates.questDesc[PatchCustomStates.questNum];
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

        static void CompleteQuest()
        {
            GrabbableObject[] temp = new GrabbableObject[0];
            Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
            int reward = PatchCustomStates.GetReward();
            int newGroupCredits = terminal.groupCredits + reward;

            terminal.groupCredits = newGroupCredits;
            HUDManager.Instance.DisplayCreditsEarning(reward, temp, newGroupCredits);

            /* TODO: Add Reward Deliveries
            terminal.orderedItemsFromTerminal.Add(1);
            terminal.BuyItemsServerRpc(terminal.orderedItemsFromTerminal.ToArray(), newGroupCredits, terminal.numberOfItemsInDropship);
            terminal.orderedItemsFromTerminal.Clear(); */
            terminal.SyncGroupCreditsClientRpc(newGroupCredits, terminal.numberOfItemsInDropship);

            PatchCustomStates.RandomizeQuest();
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
            allLines[1] = "";
            if (PatchCustomStates.taskCompleted)
            {
                allLines[2] = PatchCustomStates.questCompleteText;
                CompleteQuest();
            }
            else
            {
                allLines[2] = PatchCustomStates.questDesc[PatchCustomStates.questNum];
            }
            ((MonoBehaviour)HUDManager.Instance).StartCoroutine(ChangeControlTipMultipleDelayed(allLines));
        }

        [HarmonyPatch(nameof(Terminal.OnSubmit))]
        [HarmonyPrefix]
         static void OnSubmitPatch(ref TMP_InputField ___screenText)
        {
            if(___screenText.text.Length >= 7)
            {
                //Console.WriteLine(___screenText.text);
                if (___screenText.text.Contains("abandon"))
                {
                    PatchCustomStates.AbandonQuest();
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
            if (PatchCustomStates.questNum == 0)
            {
                string itemCollected = scrapObject.itemProperties.itemName.Substring(0, 3);
                string questItem = PatchCustomStates.GetItemName().Substring(0, 3);
                if (string.Compare(itemCollected, questItem, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PatchCustomStates.CompleteTask();
                }
            }
        }
    }

    [HarmonyPatch(typeof(HoarderBugAI))]
    internal class HoarderBugAIPatch
    {
        [HarmonyPatch(nameof(HoarderBugAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if(PatchCustomStates.questNum == 1)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(CrawlerAI))]
    internal class CrawlerAIPatch
    {
        [HarmonyPatch(nameof(CrawlerAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if(PatchCustomStates.questNum == 2)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }
}
