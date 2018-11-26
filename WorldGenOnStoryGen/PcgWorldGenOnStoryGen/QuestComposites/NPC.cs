using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public class NPC : QuestComposite
    {
        public NPC()
        {           
            types = new List<string>
            {
                "Princess",
                "Friend"
            };
            GenerateValue();
            outputComposition = type;
        }       
    }
}
