using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public class Hero : QuestComposite
    {
        public Hero()
        {
            this.type = type;
            outputComposition = type;

            types = new List<string>
            {
                "Knight"
            };
            GenerateValue();
            outputComposition = type;
        }

        public override void GenerateValue()
        {
            rnd = new Random();

          //  new Random().Shuffle<string>(types.ToArray());

            type = types[rnd.Next(0, types.Count)];
        }
    }
}
