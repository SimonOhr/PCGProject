using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class CollectAction : Action
    {
       // public Hero Hero { get; set; }
        public ObjectToCollect ThingToCollect {get;set;}
        public Location location { get; set; }
        public int NumberToCollect { get; set; }
        public CollectAction(Hero hero, ObjectToCollect thingToCollect, int numberToCollect, Location location)
        {
            Hero = hero;
            ThingToCollect = thingToCollect;
            Location = location;
            NumberToCollect = numberToCollect;
            Type = "Collect";            
            InitGlobalValues();
            // outputComposition += "FightAction: "+ Hero.outputComposition + ", " + Enemy.outputComposition + ", " + Location.outputComposition;
        }
        public override void InitGlobalValues()
        {
            objects = new ArrayList
            {
                Hero,
                ThingToCollect,
                Location
            };
            EthicsValue = 10;
            TypeOfObject = ThingToCollect.outputComposition;
        }


        public override string OutputComposition()
        {
            return "CollectAction: " + Hero.outputComposition + ", " + ThingToCollect.outputComposition + ", " + NumberToCollect + ", " + Location.outputComposition;
        }
    }
}
