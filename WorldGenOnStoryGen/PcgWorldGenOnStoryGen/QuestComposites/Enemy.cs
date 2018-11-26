using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public class Enemy : QuestComposite
    {
        public Enemy()
        {
            types = new List<string>
            {
                "Skeleton",
                "Zombie",
                "Boss"
            };
            GenerateValue();
            outputComposition = type;            
        }

        public override void GenerateValue()
        {
            rnd = new Random();

         //   new Random().Shuffle<string>(types.ToArray());

            type = types[rnd.Next(0, types.Count)];
        }
    }
}
