using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PcgWorldGenOnStoryGen
{
    class QuestGen
    {
        Quest[] quests;      
        string[] questSegments;
        Random rnd;
        int questCount;
        bool canOpen = false;
        bool hasUsed = false;
        int keys = 0;

        string lastComponent;

        public QuestGen()
        {
            questCount = 1000;
            
            quests = new Quest[questCount];
            QuestParts.BuildParts();
            questSegments = QuestParts.GetQuestParts();
            rnd = new Random();
            
            Generate();
            FileManipulator.WriteToFile(quests, true);           
        }

        void Generate()
        {
            CreateRandomizedQuests();
            for (int i = 0; i < 200; i++)
            {
                Reevaluate();
                SortFunctionalQuestsArray();
                CopyOverWorstHalf();
                Mutate();
            }
            Reevaluate();
            SortFunctionalQuestsArray();
        }

        void CreateRandomizedQuests()
        {
            for (int i = 0; i < quests.Length; i++)
            {
                Quest temp = new Quest();
                temp.SetQuest(PrepareRandomQuestSegments(temp.QuestSize));
                quests[i] = temp;
            }
        }

        string[] PrepareRandomQuestSegments(int size)
        {
            string[] temp = new string[size];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] += QuestParts.GetQuestParts(rnd.Next(0, 10));
            }
            return temp;
        }           

        void SortFunctionalQuestsArray()
        {
            for (int i = 0; i < quests.Length - 1; i++)
            {
                int min = i;
                Quest temp;
                for (int j = i + 1; j < quests.Length; j++)
                {
                    if (quests[j].Fitness > quests[min].Fitness)
                        min = j;
                }
                if (min != i)
                {
                    temp = quests[i];
                    quests[i] = quests[min];
                    quests[min] = temp;
                }
            }
        }

        void CopyOverWorstHalf()
        {          
            for (int i = 0; i < quests.Length / 2; i++)
            {
                // quests[quests.Length / 2 + i].SetQuest(Array.Copy(quests, 0, quests, (quests.Length / 2 + i), 20));
                try
                {
                    Array.Copy(quests[i].GetQuestComposition(), 0, quests[quests.Length / 2 + i].GetQuestComposition(), 0, quests[quests.Length / 2 + i].GetQuestComposition().Length);
                }
                catch (Exception)
                {
                    throw;
                }
                
            }            
        }

        void Mutate()
        {               
            int tempLength = quests.Length / 2;
            for (int i = 0; i < tempLength; i++)
            {
                new Random().Shuffle<string>(quests[tempLength + i].QuestComposition);

                int firstRandom = rnd.Next(0, quests[tempLength + i].QuestSize);
                int secondRandom = rnd.Next(0, QuestParts.GetQuestParts().Length);
                quests[tempLength + i].QuestComposition[firstRandom] = QuestParts.GetQuestParts(secondRandom);
            }
        }

        void Reevaluate()
        {
            EvaluateQuests(quests);

        }       

        void EvaluateQuests(Quest[] quests)
        {

            for (int i = 0; i < quests.Length; i++)
            {
                int checksReturned = 0;
                int checksExpected = 3;
                quests[i].Fitness = 0;
                quests[i].IsPlayable = false;
                bool hasReturned = true;


                for (int j = 0; j < quests[i].QuestComposition.Length; j++)
                {                 
                    quests[i].QuestComposition[j] = RerollImmidiatelyRepetitiveOccurances(quests[i], j);

                }                
                if(CheckForAndOrderKeysAndDoors(quests[i], "Key", "Door", 10) == false) hasReturned = false;
                if(CheckBigKeyAndBigDoorViability(quests[i]) == false) hasReturned = false;


                //AppropriateNPCHandlingFitness(quests[i], "Princess"); //TODO refactor
                //AppropriateNPCHandlingFitness(quests[i], "Knight");
                //AppropriateNPCHandlingFitness(quests[i], "Friend");

                //AppropriateEnemyHandlingFitness(quests[i], "Boss");
                AppropriateEnemyHandlingFitness(quests[i], "Zombie");
                AppropriateEnemyHandlingFitness(quests[i], "Skeleton");

                if (AppropiateObjectHandling(quests[i], "Treasure") == false) hasReturned = false;
                AppropiateObjectHandling(quests[i], "Key");
                AppropiateObjectHandling(quests[i], "Door");

                AppropiateLocationHandling(quests[i], "Dungeon");
                AppropiateLocationHandling(quests[i], "Castle");

                AppropiateActionHandling(quests[i], "Talk To");
                AppropiateActionHandling(quests[i], "Walk To");
                AppropiateActionHandling(quests[i], "Open");
                AppropiateActionHandling(quests[i], "Fight");
                AppropiateActionHandling(quests[i], "Rescue");
                AppropiateActionHandling(quests[i], "Collect");
                AppropiateActionHandling(quests[i], "Find");


                if (hasReturned == true)
                    quests[i].IsPlayable = true;
                else
                    IsNotPlayable(quests[i]);
            }
        }

        void IsNotPlayable(Quest item)
        {
            item.IsPlayable = false;

            item.Fitness = -1000;
        }

        bool CheckForAndOrderKeysAndDoors(Quest item, string keyWord, string doorWord, int fitnessAward)
        {
            List<int> keyIndexes = GetQuestComponentIndex(item, keyWord);
            List<int> doorIndexes = GetQuestComponentIndex(item, doorWord);
            bool doSwitches = false;
            int keysCount = keyIndexes.Count;

            if (keysCount > 0)
            {
                if (keysCount == doorIndexes.Count)
                {
                    doSwitches = true;
                }
            }

            if (doSwitches)
            {
                for (int i = 0; i < keysCount; i++)
                {
                    if (keyIndexes[i] > doorIndexes[i])
                    {
                        int tempKeyIndex = keyIndexes[i];
                        int tempDoorIndex = doorIndexes[i];

                        //string temp = item.QuestComposition[tempKeyIndex]; // sparar position för nyckel (som kommer fter dörr)
                       string temp = string.Copy(item.QuestComposition[tempKeyIndex]);

                        item.QuestComposition[tempKeyIndex] = item.QuestComposition[tempDoorIndex]; // om vi får nycklen efter dörren, så bytar vi ut nyckel mot en dörr.
                        item.QuestComposition[tempDoorIndex] = temp; // sätter dörr där nyckeln befann sig

                        item.Fitness += fitnessAward;
                    }
                }
            }
            return doSwitches;
        }

        bool CheckBigKeyAndBigDoorViability(Quest item)
        {
            bool isPlayable = false;
            

            List<int> bigKeyIndexes = GetQuestComponentIndex(item, "Big Key");
            List<int> bigDoorIndexes = GetQuestComponentIndex(item, "Big Door");
            int bigKeysCount = bigKeyIndexes.Count;

            if (bigKeysCount <= 2)
            {
                isPlayable = CheckForAndOrderKeysAndDoors(item, "Big Key", "Big Door", 20);
            }
            if(bigKeysCount > 2 && !isPlayable)
            {
                for (int p = 0; p < bigKeysCount; p++)
                {
                    RerollImmidiatelyRepetitiveOccurances(item, bigKeyIndexes[p]);
                }
                for (int p = 0; p < bigDoorIndexes.Count; p++)
                {
                    RerollImmidiatelyRepetitiveOccurances(item, bigDoorIndexes[p]);
                }                
            }
                

            if (isPlayable)
            {
                for (int i = 0; i < bigKeysCount; i++)
                {
                    if (bigKeyIndexes[i] - 2 >= 0)
                    {
                        if (item.QuestComposition[bigKeyIndexes[i] - 2] == "Walk To")
                            item.Fitness += 10;
                    }
                    if (bigKeyIndexes[i] - 1 >= 0)
                    {
                        if (item.QuestComposition[bigKeyIndexes[i] - 1] == "Open")
                            item.Fitness += 10;
                    }
                }
            }          
            return true;
        }

        List<int> GetQuestComponentIndex(Quest item, string value)
        {
            List<int> indexes = new List<int>();
            bool isSearching = true;
            int oldIndex = -1;
            //int it = 0;
            for (int it = 0; it < item.QuestComposition.Length; it++)
            {
                int index = Array.FindIndex(item.QuestComposition, it, x => x == value);
                if (index != -1 && index != oldIndex)
                {
                    indexes.Add(index);
                    oldIndex = index;
                }
            }
            return indexes;
        }

        /// <summary>
        /// Rerolls quest component if repeated immidietly after a quest component of the same type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="wordIt"></param>
        /// <returns></returns>
        string RerollImmidiatelyRepetitiveOccurances(Quest item, int wordIt)
        {
            if (lastComponent != null) // första iterationen kommer vara null
            {
                while (item.QuestComposition[wordIt] == lastComponent)
                {
                    item.QuestComposition[wordIt] = QuestParts.GetQuestParts(rnd.Next(0, 10));
                }
            }
            lastComponent = item.QuestComposition[wordIt];
            return item.QuestComposition[wordIt];
        }

        void AppropiateLocationHandling(Quest item, string obj)
        {
            List<int> objIndexes = GetQuestComponentIndex(item, obj);
            bool doSearch = false;
            int length = objIndexes.Count;

            if (length > 0)
            {
                int it = 1;
                doSearch = true;

                while (it < length)
                {
                    item.Fitness -= 10;
                    it++;
                }
            }

            int treasureCount = 0;
            if (doSearch)
            {
                for (int i = 0; i < length; i++)
                {
                    if (objIndexes[i] - 1 > 0)
                    {
                        if (item.QuestComposition[objIndexes[i] - 1] == "Walk To")
                        {
                            item.Fitness += 10;
                        }
                    }
                    if (objIndexes[i] + 1 < length)
                    {
                        if (item.QuestComposition[objIndexes[i]] == "Dungeon")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Open")
                            {
                                item.Fitness += 10;
                            }
                        }                       
                        else if (item.QuestComposition[objIndexes[i] + 1] == "Find")
                        {
                            item.Fitness += 10;
                        }
                    }
                }
            }
        }
        bool AppropiateActionHandling(Quest item, string obj)
        {
            List<int> objIndexes = GetQuestComponentIndex(item, obj);
            bool doSearch = false;
            int length = objIndexes.Count;

            if (length > 0)
            {
                int it = 1;
                doSearch = true;

                while (it < length)
                {
                    item.Fitness -= 10;
                    it++;
                }
            }
            int talkToIt = 0;
            int walkToIt = 0;
            int openIt = 0;
            int fightIt = 0;
            int bossIt = 0;
            int findIt = 0;
            int rescueIt = 0;
            int collectIt = 0;

            if (doSearch)
            {
                for (int i = 0; i < length; i++)
                {
                    if (objIndexes[i] + 1 < length)
                    {
                        if (item.QuestComposition[objIndexes[i]] == "Talk To")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Princess")
                            {
                                item.Fitness += 30;
                                talkToIt++;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Knight")
                            {
                                item.Fitness += 30;
                                talkToIt++;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Friend")
                            {
                                item.Fitness += 30;
                                talkToIt++;
                            }
                            //if(talkToIt > 3)
                            //{
                            //    item.Fitness -= 20;
                            //}
                            
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Walk To")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Princess")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Knight")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Friend")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Castle")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Dungeon")
                            {
                                item.Fitness += 30;
                            }
                            else if(item.QuestComposition[objIndexes[i] + 1] == "Boss")
                            {
                                item.Fitness += 20;
                            }
                            //if (walkToIt > 4)
                            //{
                            //    item.Fitness -= 20;
                            //}
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Open")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Door")
                            {
                                item.Fitness += 40;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Big Door")
                            {
                                item.Fitness += 40;
                            }
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Fight")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Princess")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Knight")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Friend")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Zombie")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Skeleton")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Boss")
                            {
                                item.Fitness += 50;
                                bossIt++;
                            }
                            if(bossIt > 2)
                            {
                                item.Fitness -= 50;
                            }
                            //if (fightIt > 5)
                            //{
                            //    item.Fitness -= 20;
                            //}
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Find")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Princess")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Knight")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Friend")
                            {
                                item.Fitness += 10;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Boss")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Door")
                            {
                                item.Fitness += 40;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Big Door")
                            {
                                item.Fitness += 40;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Key")
                            {
                                item.Fitness += 40;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Big Key")
                            {
                                item.Fitness += 40;
                            }
                            //if (findIt > 2)
                            //{
                            //    item.Fitness -= 20;
                            //}
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Rescue")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Princess")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Knight")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Friend")
                            {
                                item.Fitness += 30;
                            }
                            //if (rescueIt > 2)
                            //{
                            //    item.Fitness -= 20;
                            //}
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Collect")
                        {
                            if (item.QuestComposition[objIndexes[i] + 1] == "Treasure")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Key")
                            {
                                item.Fitness += 30;
                            }
                            else if (item.QuestComposition[objIndexes[i] + 1] == "Big Key")
                            {
                                item.Fitness += 30;
                            }
                            //if (walkToIt > 5)
                            //{
                            //    item.Fitness -= 20;
                            //}
                        }
                    }
                }
            }
            return doSearch;
        }

        bool AppropiateObjectHandling(Quest item, string obj)
        {
            List<int> objIndexes = GetQuestComponentIndex(item, obj);
            bool doSearch = false;
            int length = objIndexes.Count;

            if (length > 0)
            {
                int it = 1;
                doSearch = true;

                while (it < length)
                {
                    item.Fitness -= 10;
                    it++;
                }
            }

            int treasureCount = 0;
            if (doSearch)
            {
                for (int i = 0; i < length; i++)
                {
                    if (objIndexes[i] - 1 > 0)
                    {
                        if (item.QuestComposition[objIndexes[i] - 1] == "Find")
                        {
                            item.Fitness += 10;
                        }

                        else if (item.QuestComposition[objIndexes[i] - 1] == "Get")
                        {
                            item.Fitness += 10;
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Treasure")
                        {
                            if (item.QuestComposition[objIndexes[i] - 1] == "Collect")
                            {
                                item.Fitness += 10;
                                treasureCount++;
                                if (treasureCount <= 5)
                                {
                                    item.Fitness -= 10;
                                   
                                }
                                else
                                {
                                    doSearch = false;
                                    break;
                                }
                            }
                        }
                        else if (item.QuestComposition[objIndexes[i]] == "Door")
                        {
                            if (item.QuestComposition[objIndexes[i] - 1] == "Open")
                            {
                                item.Fitness += 20;
                            }
                        }
                    }
                    if (objIndexes[i] - 2 > 0)
                    {
                        if (item.QuestComposition[objIndexes[i] - 2] == "Walk To")
                        {
                            item.Fitness += 10;
                        }
                    }
                }
            }
            return doSearch;
        }

        bool AppropriateEnemyHandlingFitness(Quest item, string NPC)
        {
            List<int> npcIndexes = GetQuestComponentIndex(item, NPC);
            bool doSearch = false;
            int length = npcIndexes.Count;

            if (length > 0)
            {
                doSearch = true;
                int it = 0;
                while (it < length)
                {
                    item.Fitness -= 10;
                    it++;
                }
            }

            int fightCount = 0;
            if (doSearch)
            {
                for (int i = 0; i < length; i++)
                {
                    if (npcIndexes[i] - 1 > 0)
                    {
                        if (item.QuestComposition[npcIndexes[i] - 1] == "Walk To")
                        {
                            item.Fitness += 10;
                        }

                        else if (item.QuestComposition[npcIndexes[i] - 1] == "Find")
                        {
                            item.Fitness += 10;
                        }

                        else if (item.QuestComposition[npcIndexes[i] - 1] == "Fight")
                        {
                            item.Fitness += 10;
                            if (fightCount++ > 5)
                            {
                                doSearch = false;
                                break;
                            }
                        }
                    }
                    if (npcIndexes[i] + 1 < length)
                    {
                        if (item.QuestComposition[npcIndexes[i] + 1] == "Get")
                        {
                            item.Fitness += 10;
                        }
                    }
                }
            }
            return doSearch;
        }
        void AppropriateNPCHandlingFitness(Quest item, string NPC)
        {
            List<int> npcIndexes = GetQuestComponentIndex(item, NPC);
            bool doSearch = false;
            int length = npcIndexes.Count;

            if (length > 0)
            {
                doSearch = true;
                int it = 0;
                while (it < length)
                {
                    item.Fitness -= 10;
                    it++;
                }
            }


            int rescueCount = 0;
            int fightCount = 0;
            if (doSearch)
            {
                for (int i = 0; i < length; i++)
                {
                    if (npcIndexes[i] - 1 > 0)
                    {
                        if (item.QuestComposition[npcIndexes[i] - 1] == "Talk To")
                        {
                            item.Fitness += 10;
                        }

                        else if (item.QuestComposition[npcIndexes[i] - 1] == "Walk To")
                        {
                            item.Fitness += 10;
                        }

                        else if (item.QuestComposition[npcIndexes[i] - 1] == "Find")
                        {
                            item.Fitness += 10;
                        }

                        else if (item.QuestComposition[npcIndexes[i] - 1] == "Fight")
                        {
                            item.Fitness += 10;
                            if (fightCount++ > 1)
                            {
                                item.Fitness -= 10;
                            }
                        }

                        else if (item.QuestComposition[npcIndexes[i] - 1] == "Rescue")
                        {
                            item.Fitness += 10;
                            if (rescueCount++ > 1)
                            {
                                item.Fitness -= 10;
                            }

                        }
                    }
                }
            }

        }
    }
}
