using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PcgWorldGenOnStoryGen
{
    public class TalkToAction : Action
    {
       // public QuestComposite Hero { get; set; }
        public QuestComposite Npc { get; set; }
       // public QuestComposite Location { get; set; }
        public TalkToAction(Hero hero, NPC npc, Location location)
        {
            Hero = hero;
            Npc = npc;
            Location = location;          
            InitGlobalValues();
            EthicsValue = 5;           
        }
        public TalkToAction(Hero hero, Enemy npc, Location location)
        {
            Hero = hero;
            Npc = npc;
            Location = location;
            InitGlobalValues();
            EthicsValue = -5;           
        }

        public override void InitGlobalValues()
        {
            Type = "Talk";
            objects = new ArrayList
            {
                Hero,
                Npc,
                Location
            };            
            TypeOfFriend = Npc.outputComposition;
        }

        public override string OutputComposition()
        {
            return "TalkToAction: " + Hero.outputComposition + ", " + Npc.outputComposition + ", " + Location.outputComposition;
        }
    }
}
