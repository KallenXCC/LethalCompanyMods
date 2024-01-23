namespace SideQuests.Patches
{
    public static class SQCustomStates
    {
        public static string[] questDesc = new string[3];
        public static int questID = 0;
        public static int itemID = 0;
        public static int enemyID = 0;
        public static int secretID = 0;
        public static bool taskCompleted = false;

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

        static SQCustomStates()
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
            HUDManager.Instance.DisplayTip("New Quest", questDesc[questID]);
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
            //HUDManager.Instance.DisplayTip("New Quest", questDesc[questID]);
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
}


