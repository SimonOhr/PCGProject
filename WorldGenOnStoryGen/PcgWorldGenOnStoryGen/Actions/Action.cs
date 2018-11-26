using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public abstract class Action
    {
        public string outputComposition;

        public ArrayList objects;

        public Location Location { get; set; }

        public Hero Hero { get; set; }

        public int EthicsValue { get; protected set; }

        public string Type { get; protected set; }

        public string TypeOfEnemy { get; protected set; }

        public string TypeOfFriend { get; protected set; }

        public string TypeOfObject { get; protected set; }

        public string TypeOfLocation { get; protected set; }

        public virtual void InitGlobalValues()
        {

        }


        public virtual string OutputComposition()
        {
            return outputComposition;
        }

    }


}
