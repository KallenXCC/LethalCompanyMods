using GameNetcodeStuff;
using HarmonyLib;
using TMPro;
using System;
/* using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BepInEx.Logging; */
using UnityEngine;
using System.Collections;
/* using System.Reflection;
using System.Xml.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Windows; */
using Unity.Netcode;

namespace SideQuests.Patches
{
    public static class PatchCustomStates
    {
        public static string[] questDesc = new string[3];
        public static int questID = 0;
        public static int itemID = 0;
        public static int enemyID = 0;
        public static int secretID = 0;
        public static bool taskCompleted = false;

        public static string turnInQuest = "Task Completed! \nUse Terminal to Complete";
        public static string questCompleteText = "Quest Completed!";
        public static string abandonQuestText = "\nEnter 'abandon' in terminal\nto abandon quest";

        readonly static string[] itemList = new string[45];
        readonly static string[] enemyList = new string[11];
        public const int MANTICOIL_ID = 0;
        public const int LOOTBUG_ID = 1;
        public const int FLEA_ID = 2;
        public const int THUMPER_ID = 3;
        public const int MASKED_ID = 4;
        public const int SPIDER_ID = 5;
        public const int BABOON_ID = 6;
        public const int NUTCRACKER_ID = 7;
        public const int BRACKEN_ID = 8;
        public const int DOG_ID = 9;
        public const int GIANT_ID = 10;
        readonly static string[] secretList = new string[9];
        public const int SLIME_SID = 0;
        public const int SPORE_SID = 1;
        public const int GHOST_SID = 2;
        public const int MASK_SID = 3;
        public const int TURRET_SID = 4;
        public const int LANDMINE_SID = 5;
        public const int GIFT_SID = 6;
        public const int FART_SID = 7;
        public const int ELECTRICITY_SID = 8;

        static System.Random random = new System.Random();

        static PatchCustomStates()
        {
            itemList[0] = "Gift Box";
            itemList[1] = "Big bolt";
            itemList[2] = "V-type engine";
            itemList[3] = "Large axle";
            itemList[4] = "Metal sheet";
            itemList[5] = "Bottles";
            itemList[6] = "Tea kettle";
            itemList[7] = "Bell";
            itemList[8] = "Plastic fish";
            itemList[9] = "Flask";
            itemList[10] = "Toy cube";
            itemList[11] = "Magnifying glass";
            itemList[12] = "Hair brush";
            itemList[13] = "Stop sign";
            itemList[14] = "Cookie mold pan";
            itemList[15] = "Dust pan";
            itemList[16] = "Clown horn";
            itemList[17] = "Coffee mug";
            itemList[18] = "Tragedy";
            itemList[19] = "Airhorn";
            itemList[20] = "Comedy";
            itemList[21] = "Fancy lamp";
            itemList[22] = "Red soda";
            itemList[23] = "Egg beater";
            itemList[24] = "Whoopie-Cushion";
            itemList[25] = "DIY-Flashbang";
            itemList[26] = "Teeth";
            itemList[27] = "Toothpaste";
            itemList[28] = "Old phone";
            itemList[29] = "Jar of pickles";
            itemList[30] = "Rubber Ducky";
            itemList[31] = "Remote";
            itemList[32] = "Steering wheel";
            itemList[33] = "Golden cup";
            itemList[34] = "Yield sign";
            itemList[35] = "Perfume bottle";
            itemList[36] = "Ring";
            itemList[37] = "Laser pointer";
            itemList[38] = "Chemical jug";
            itemList[39] = "Painting";
            itemList[40] = "Cash register";
            itemList[41] = "Candy";
            itemList[42] = "Pill bottle";
            itemList[43] = "Apparatus";
            itemList[44] = "Hive";

            enemyList[MANTICOIL_ID] = "Manticoil";
            enemyList[LOOTBUG_ID] = "Loot Bug";
            enemyList[FLEA_ID] = "Snare Flea";
            enemyList[THUMPER_ID] = "Thumper";
            enemyList[SPIDER_ID] = "Spider";
            enemyList[MASKED_ID] = "Masked";
            enemyList[BABOON_ID] = "Baboon Hawk";
            enemyList[NUTCRACKER_ID] = "Nutcracker";
            enemyList[BRACKEN_ID] = "Bracken";
            enemyList[DOG_ID] = "Eyeless Dog";
            enemyList[GIANT_ID] = "Forest Giant";

            secretList[SLIME_SID] = "Slime Buddy";
            secretList[SPORE_SID] = "Pink Fumes";
            secretList[GHOST_SID] = "Haunted Chase";
            secretList[MASK_SID] = "Drama";
            secretList[TURRET_SID] = "Angry Turret";
            secretList[LANDMINE_SID] = "BOOM";
            secretList[GIFT_SID] = "Happy Birthday!";
            secretList[FART_SID] = "Toot";
            secretList[ELECTRICITY_SID] = "Ben Franklin";

            RandomizeQuest();

            questID = 2;
            secretID = ELECTRICITY_SID;
            questDesc[2] = "Secret Quest:\n" + secretList[secretID];
        }

        public static void RandomizeQuest()
        {
            taskCompleted = false;
            int randomNumber = random.Next(0, 10);
            if (randomNumber < 7) // item quest 70%
            {
                questID = 0;
                randomNumber = random.Next(0, 10);
                if (randomNumber < 6) // easy item 60%
                {
                    randomNumber = random.Next(0, 10);
                } else if (randomNumber < 9) // medium item 30%
                {
                    randomNumber = random.Next(10, 26);
                } else // hard item 10%
                {
                    randomNumber = random.Next(26, itemList.Length);
                }
                itemID = randomNumber;
                questDesc[0] = "Collect Quest:\n" + itemList[itemID];
            } else if (randomNumber < 9) // kill quest 20%
            {
                questID = 1;
                randomNumber = random.Next(0, 10);
                if (randomNumber < 5) // easy enemy 50%
                {
                    randomNumber = random.Next(MANTICOIL_ID, THUMPER_ID);
                }
                else if (randomNumber < 9) // medium enemy 40%
                {
                    randomNumber = random.Next(THUMPER_ID, BRACKEN_ID);
                }
                else // hard enemy 10%
                {
                    randomNumber = random.Next(BRACKEN_ID, enemyList.Length);
                }
                enemyID = randomNumber;
                questDesc[1] = "Kill Quest:\n" + enemyList[enemyID];
            } else // secret quest 10%
            {
                questID = 2;
                randomNumber = random.Next(0, secretList.Length);
                secretID = randomNumber;
                questDesc[2] = "Secret Quest:\n" + secretList[secretID];
            }
        }

        public static void CompleteTask()
        {
            taskCompleted = true;
            HUDManager.Instance.ClearControlTips();
            HUDManager.Instance.DisplayTip("Task Completed", questDesc[questID]);
        }

        public static string GetItemName()
        {
            return itemList[itemID];
        }

        public static int GetReward()
        {
            int reward = 50 + questID * 50;
            if (questID == 0)
            {
                reward += itemID * 3;
            } else if (questID == 1)
            {
                reward += enemyID * 25;
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
                ___controlTipLines[0].text = PatchCustomStates.questDesc[PatchCustomStates.questID];
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
            Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
            int reward = PatchCustomStates.GetReward();
            int newGroupCredits = terminal.groupCredits + reward;

            GrabbableObject[] questReward = new GrabbableObject[0];
            /*questReward[0].itemProperties.itemName = PatchCustomStates.questDesc[PatchCustomStates.questID];
            questReward[0].itemProperties.isScrap = true;
            questReward[0].scrapValue = reward;*/

            terminal.groupCredits = newGroupCredits;
            HUDManager.Instance.DisplayCreditsEarning(reward, questReward, newGroupCredits);
            StartOfRound.Instance.gameStats.scrapValueCollected += reward;
            TimeOfDay.Instance.quotaFulfilled += reward;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
            /* TODO: Add Reward Deliveries
            terminal.orderedItemsFromTerminal.Add(1);
            terminal.BuyItemsServerRpc(terminal.orderedItemsFromTerminal.ToArray(), newGroupCredits, terminal.numberOfItemsInDropship);
            terminal.orderedItemsFromTerminal.Clear(); */
            NetworkManager networkManager = terminal.NetworkManager;
            if(networkManager != null && networkManager.IsServer)
            {
                terminal.SyncGroupCreditsClientRpc(newGroupCredits, terminal.numberOfItemsInDropship);
            } else
            {
                terminal.SyncGroupCreditsServerRpc(newGroupCredits, terminal.numberOfItemsInDropship);
            }
            
            DepositItemsDesk depositItemsDesk = UnityEngine.Object.FindObjectOfType<DepositItemsDesk>();
            /* depositItemsDesk.AddObjectToDeskServerRpc(NetworkObjectReference.op_Implicit(((Component)playerWhoTriggered.currentlyHeldObjectServer).gameObject.GetComponent<NetworkObject>()));
            if (depositItemsDesk != null)
            {
                depositItemsDesk.SellItemsOnServer();
            } */

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
                allLines[1] = PatchCustomStates.questDesc[PatchCustomStates.questID];
                allLines[2] = PatchCustomStates.abandonQuestText;
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
            if (PatchCustomStates.questID == 0)
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

    [HarmonyPatch(typeof(DoublewingAI))]
    internal class DoublewingAIPatch
    {
        [HarmonyPatch(nameof(DoublewingAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.MANTICOIL_ID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.LOOTBUG_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(CentipedeAI))]
    internal class CentipedeAIPatch
    {
        [HarmonyPatch(nameof(CentipedeAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.FLEA_ID)
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
            if(PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.THUMPER_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(SandSpiderAI))]
    internal class SandSpiderAIPatch
    {
        [HarmonyPatch(nameof(SandSpiderAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.SPIDER_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(MaskedPlayerEnemy))]
    internal class MaskedPlayerEnemyPatch
    {
        [HarmonyPatch(nameof(MaskedPlayerEnemy.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.MASKED_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(BaboonBirdAI))]
    internal class BaboonBirdAIPatch
    {
        [HarmonyPatch(nameof(BaboonBirdAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.BABOON_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(NutcrackerEnemyAI))]
    internal class NutcrackerEnemyAIPatch
    {
        [HarmonyPatch(nameof(NutcrackerEnemyAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.NUTCRACKER_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(FlowermanAI))]
    internal class FlowermanAIPatch
    {
        [HarmonyPatch(nameof(FlowermanAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.BRACKEN_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(MouthDogAI))]
    internal class MouthDogAIPatch
    {
        [HarmonyPatch(nameof(MouthDogAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.DOG_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(ForestGiantAI))]
    internal class ForestGiantAIPatch
    {
        [HarmonyPatch(nameof(ForestGiantAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (PatchCustomStates.questID == 1 && PatchCustomStates.enemyID == PatchCustomStates.GIANT_ID)
            {
                PatchCustomStates.CompleteTask();
            }
        }
    }

    [HarmonyPatch(typeof(BlobAI))]
    internal class BlobAIPatch
    {
        [HarmonyPatch(nameof(BlobAI.DetectNoise))]
        [HarmonyPostfix]
        static void DetectNoisePatch(ref float ___tamedTimer)
        {
            if (PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.SLIME_SID)
            {
                if (___tamedTimer > 0 && !PatchCustomStates.taskCompleted)
                {
                    PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.SPORE_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.GHOST_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.MASK_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.TURRET_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.LANDMINE_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if (PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.GIFT_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.FART_SID)
            {
                PatchCustomStates.CompleteTask();
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.ELECTRICITY_SID)
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
                for(int i = 0; i < playerControllerB.ItemSlots.Length; i++)
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
            if(PatchCustomStates.questID == 2 && PatchCustomStates.secretID == PatchCustomStates.ELECTRICITY_SID)
            {
                if(aliveBeforeLightning && holdingAKey)
                {
                    PlayerControllerB playerControllerB = GameNetworkManager.Instance.localPlayerController;
                    if(playerControllerB.isPlayerDead)
                    {
                        PatchCustomStates.CompleteTask();
                    }
                }
            }
        }
    }
}


