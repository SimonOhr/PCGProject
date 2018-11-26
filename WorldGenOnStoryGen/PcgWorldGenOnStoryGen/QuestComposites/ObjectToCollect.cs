using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class ObjectToCollect:QuestComposite
    {
        public ObjectToCollect()
        {
            types = new List<string>
            {
                "Coins",
                "Bones",
                "Key",
                "Flower"
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
