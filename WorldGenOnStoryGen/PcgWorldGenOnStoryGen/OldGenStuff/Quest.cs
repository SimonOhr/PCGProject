using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public class Quest
    {        
        string[] questComposition;
        int fitness;
        bool isPlayable = false;
        int questSize;  

        public int QuestSize { get => questSize; set => questSize = value; }

        public string[] QuestComposition { get { return questComposition; } set { questComposition = value; } }

        public int Fitness { get => fitness; set => fitness = value; }

        public bool IsPlayable { get => isPlayable; set => isPlayable = value; }

        //DEBUG VALUES
        public int id;

        public Quest()
        {
            int rnd = new Random().Next(30, 50);
            questSize = rnd;
            fitness = 0;            
            questComposition = new string[questSize];
           
        }    
        
        public void SetQuest(string[] _questComposition)
        {
            for (int i = 0; i < _questComposition.Length; i++)
            {
                questComposition[i] += _questComposition[i];
            }
        }        

        public string[] GetQuestComposition()
        {
            return questComposition;
        }
    }
}
