using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class QuestEnsureLogicalOrder
    {
        public QuestEnsureLogicalOrder(List<DebugQuest> quests)
        {
            foreach (DebugQuest item in quests)
            {
                FindRelevantActions(item);
            }
        }

        void FindRelevantActions(DebugQuest quest)
        {
            List<FightAction> fights = new List<FightAction>();
            List<int> keys = new List<int>();
            List<int> doors = new List<int>();
            List<int> chests = new List<int>();
           

            for (int i = 0; i < quest.actionArray.Length; ++i)
            {
                if (quest.actionArray[i] is FightAction)
                {
                    fights.Add(quest.actionArray[i] as FightAction);
                }
                else if (quest.actionArray[i] is CollectAction)
                {
                    if (quest.actionArray[i].TypeOfObject == "Key")
                        keys.Add(i);
                }
                else if (quest.actionArray[i] is OpenAction)
                {
                    if (quest.actionArray[i].TypeOfObject == "Door")
                        doors.Add(i);
                    if (quest.actionArray[i].TypeOfObject == "Chest")
                        chests.Add(i);
                }
            }           
            if (keys.Count > (chests.Count + doors.Count))
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    quest.actionArray[keys[i]] = NewCollectAction((CollectAction)quest.actionArray[keys[i]]);
                }
                for (int i = 0; i < doors.Count; i++)
                {
                    quest.actionArray[keys[i]] = NewOpenAction((OpenAction)quest.actionArray[doors[i]]);
                }
            }                

            CheckForAndOrderKeysAndDoors(quest, keys, doors); // amke sure to send only keys to doors not chests
               
        }

        CollectAction NewCollectAction(CollectAction action)
        {
            while (action.ThingToCollect.outputComposition == "Key")
            {
                action = ActionGen.NewCollectAction();
            }
            return action;
        }

        OpenAction NewOpenAction(OpenAction action)
        {
            while (action.ObjectToOpen.outputComposition == "Door")
            {
                action = ActionGen.NewOpenAction();
            }
            return action;
        }

        void CheckForAndOrderKeysAndDoors(DebugQuest quest, List<int> key, List<int> door)
        {
            List<int> keyIndexes =  key;
            List<int> doorIndexes = door;
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
                        Action temp = quest.actionArray[tempKeyIndex];

                        quest.actionArray[tempKeyIndex] = quest.actionArray[tempDoorIndex]; // om vi får nycklen efter dörren, så bytar vi ut nyckel mot en dörr.
                        quest.actionArray[tempDoorIndex] = temp; // sätter dörr där nyckeln befann sig                       
                    }
                }
            }           
        }      
    }
}
