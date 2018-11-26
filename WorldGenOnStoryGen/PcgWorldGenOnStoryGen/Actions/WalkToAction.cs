using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PcgWorldGenOnStoryGen
{
    public class WalkToAction: Action
    {
      //  public QuestComposite Hero { get; private set; }
        Hero hero;
       // public QuestComposite Location { get; set; }
        public WalkToAction(Hero hero, Location location)
        {
            this.hero = hero;
            Hero = hero;
            Location = location;
            Type = "Walk";
            InitGlobalValues();
            //outputComposition = "WalkTo: " + Hero.outputComposition + ", " + Location.outputComposition;
        }

        public override void InitGlobalValues()
        {
            objects = new ArrayList
            {
                Hero,                
                Location
            };
            EthicsValue = 0;
            TypeOfLocation = Location.outputComposition;
        }

        public override string OutputComposition()
        {
            return "WalkTo: " + Hero.outputComposition + ", " + Location.outputComposition;
        }
    }
}
