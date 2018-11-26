using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class QuestsEval
    {
        List<DebugQuest> quests;
        int iterations;
        Random rnd;
        ActionGen actionGen; // could pass existing instance
        public QuestsEval(ref List<DebugQuest> quests)
        {
            this.quests = quests;
            InitVariables();
        }

        void InitVariables()
        {
            iterations = 10000;
            rnd = new Random();
            actionGen = new ActionGen();
        }

        public List<DebugQuest> FlowHandler()
        {
            for (int i = 0; i < iterations; i++)
            {
                Eval();
                SortQuests();
                CopyGoodPart();
                Mutate();
            }
            SortQuests();

            for (int i = 0; i < quests.Count; i++)
            {
                for (int j = 0; j < quests[i].actionArray.Length; j++)
                {
                    Console.WriteLine($"QuestAction: {quests[i].actionArray[j].Type}, consisting of: {quests[i].actionArray[j].OutputComposition()}\t");
                }
                Console.WriteLine($"Quest affinity: isEvil={quests[i].IsEvil} FitnessValue: {quests[i].Fitness}, EthicValue {quests[i].EthicsValue}");
            }            

            return quests;
        }      

        void Eval()
        {
            foreach (DebugQuest item in quests)
            {
                item.Fitness = 0;
                item.EthicsValue = 0;                            
            }
            new QuestEnsureLogicalOrder(quests);

            new QuestEthicsEvaluation(quests);
            new QuestVarietyEvaluation(quests);
            new QuestEnemiesEvaluation(quests);          
        }

        void SortQuests()
        {
            for (int i = 0; i < quests.Count - 1; i++)
            {
                int min = i;
                DebugQuest temp;
                for (int j = i + 1; j < quests.Count; j++)
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

        void CopyGoodPart()
        {
            for (int i = 0; i < quests.Count / 2; i++)
            {
                try
                {
                    Action[] temp = new Action[quests[i].actionArray.Length];
                    Array.Copy(quests[i].actionArray, temp, quests[i].actionArray.Length);
                    quests[quests.Count / 2 + i].actionArray = temp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        void Mutate()
        {
            int halfLength = quests.Count / 2;
            for (int i = 0; i < halfLength; i++)
            {
                new Random().Shuffle(quests[halfLength + i].actionArray);

                int firstRandom = rnd.Next(0, quests[halfLength + i].actionArray.Length);
                quests[halfLength + i].actionArray[firstRandom] = actionGen.GetNewAction(quests[halfLength + i], quests[halfLength + i].actionArray[firstRandom].Location, quests[halfLength + i].actionArray[firstRandom].Hero);               
            }
        }
    }
}
