using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PcgWorldGenOnStoryGen
{
    public class FightAction : Action
    {
       // public QuestComposite Hero { get; set; }
        public QuestComposite Enemy { get; set; }       
       // public QuestComposite Location { get; set; }

        public FightAction(Hero hero, Enemy enemy, Location location)
        {
            Hero = hero;
            Enemy = enemy;
            Location = location;         
            InitGlobalValues();
            // outputComposition += "FightAction: "+ Hero.outputComposition + ", " + Enemy.outputComposition + ", " + Location.outputComposition;
            EthicsValue = 10;
        }
        public FightAction(Hero hero, NPC enemy, Location location)
        {
            Hero = hero;
            Enemy = enemy;
            Location = location;
            InitGlobalValues();
            // outputComposition += "FightAction: "+ Hero.outputComposition + ", " + Enemy.outputComposition + ", " + Location.outputComposition;
            EthicsValue = -30;
        }

        public override void InitGlobalValues()
        {
            Type = "Fight";
            objects = new ArrayList
            {
                Hero,
                Enemy,                
                Location
            };
            
            TypeOfEnemy = Enemy.outputComposition;
            TypeOfLocation = Location.outputComposition;
        }

        public override string OutputComposition()
        {
            return "FightAction: " + Hero.outputComposition + ", " + Enemy.outputComposition + ", " + Location.outputComposition;
        }

    }
}
