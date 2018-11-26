using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class DebugQuest
    {
       // public List<Action> actionsList;
        public Action[] actionArray;
        public int Fitness { get; set; }
        public int QuestSize { get; set; }
        public int EthicsValue { get; set; }
        public bool IsEvil { get; private set; }
        public int PrimaryTargetCounter { get; set; }
       
        public DebugQuest(List<Action> actions, bool isEvil)
        {
           // this.actionsList = actions;
            IsEvil = isEvil;
            actionArray = actions.ToArray();
            QuestSize = actions.Count;
            PrintToConsole();
        }       

        void PrintToConsole()
        {
            //Console.WriteLine("DebugQuest; ");
            //for (int i = 0; i < actions.Count; i++)
            //{               
            //    Console.WriteLine(actions[i].outputComposition);                
            //}            
        }
    }
}
