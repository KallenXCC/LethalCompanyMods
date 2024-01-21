using HarmonyLib;

namespace SideQuests.Patches
{
    [HarmonyPatch(typeof(DoublewingAI))]
    internal class DoublewingAIPatch
    {
        [HarmonyPatch(nameof(DoublewingAI.KillEnemy))]
        [HarmonyPostfix]
        static void KillEnemyPatch()
        {
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.MANTICOIL_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.LOOTBUG_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.FLEA_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.THUMPER_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.SPIDER_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.MASKED_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.BABOON_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.NUTCRACKER_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.BRACKEN_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.DOG_ID)
            {
                SQCustomStates.CompleteTask();
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
            if (SQCustomStates.questID == 1 && SQCustomStates.enemyID == SQCustomStates.GIANT_ID)
            {
                SQCustomStates.CompleteTask();
            }
        }
    }
}
