using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class OpenAction : Action
    {    
        public ObjectToOpen ObjectToOpen { get; set; }
        public OpenAction(Hero hero, ObjectToOpen objectToOpen, Location location)
        {
            Hero = hero;
            this.ObjectToOpen = objectToOpen;
            Location = location;
        }
        public override void InitGlobalValues()
        {
            Type = "ObjectToOpen";
            objects = new ArrayList
            {
                Hero,
                ObjectToOpen,
                Location
            };
            EthicsValue = 10;
            TypeOfObject = ObjectToOpen.outputComposition;
            TypeOfLocation = Location.outputComposition;            
        }

        public override string OutputComposition()
        {
            return "OpenAction: "
                + Hero.outputComposition + ", " 
                + ObjectToOpen.outputComposition + ", " 
                + Location.outputComposition;
        }       
    }
}
