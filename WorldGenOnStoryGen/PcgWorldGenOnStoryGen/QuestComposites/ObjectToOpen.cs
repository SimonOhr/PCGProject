using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class ObjectToOpen: QuestComposite
    {
        public ObjectToOpen()
        {
            types = new List<string>
            {
                "Door",
                "Chest"                
            };
            GenerateValue();
            outputComposition = type;
        }

        public override void GenerateValue()
        {
            rnd = new Random();

            type = types[rnd.Next(0, types.Count)];
        }
    }
}
