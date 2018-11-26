using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public abstract class QuestComposite
    {
        public string outputComposition;
        protected string type;
        protected List<string> types;
        public Random rnd;       

        public virtual void GenerateValue()
        {
            rnd = new Random();            

            //new Random().Shuffle<string>(types.ToArray());

            type = types[rnd.Next(0, types.Count)];            
        }

        public virtual void NewType()
        {
            type = types[rnd.Next(0, types.Count)];
        }
    }
}
