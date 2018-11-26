using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PcgWorldGenOnStoryGen
{
    class ActionGen
    {
        static List<Action> avalableActions;
        List<Action> choosenActions;
        static Hero currentHero;
        static Location currentLocation;
        Action selectedAction;
        static Random rnd;
        int minSize, maxSize;
        bool isEvil;
        public ActionGen()
        {
            rnd = new Random();
            minSize = 5;
            maxSize = 10;
        }

        public DebugQuest GetNewQuest()
        {
            return NewQuest();
        }

        public Action GetNewAction(DebugQuest quest, Location loc, Hero hero)
        {
            bool questAffinity = quest.IsEvil;
            currentHero = hero;
            currentLocation = loc;
            RerollActionTable(questAffinity);
            selectedAction = avalableActions[rnd.Next(0, avalableActions.Count)];
            if (selectedAction is WalkToAction)
                SetNewLocation();
            return selectedAction;
        }

        DebugQuest NewQuest()
        {
            currentHero = new Hero();
            currentLocation = new Location();
            if (rnd.Next(0, 2) == 1)
                isEvil = true;
            RerollActionTable(isEvil);
            choosenActions = new List<Action>();

            PickOutActions();

            return new DebugQuest(choosenActions, isEvil);
        }       

        void RerollActionTable(bool isEvil)
        {
            if (!isEvil)
            {
                avalableActions = new List<Action> {
                new FightAction(currentHero, new Enemy(), currentLocation),
               // new WalkToAction(currentHero, currentLocation),
                new TalkToAction(currentHero, new NPC(), new Location()),
                new CollectAction(currentHero, new ObjectToCollect(), rnd.Next(1,4) , new Location()), // Eval Funktion WE SHOULD MOVE TO THIS NEW POSITION
                new OpenAction(currentHero, new ObjectToOpen(), currentLocation)
                };
            }
            else
            {
                avalableActions = new List<Action>
                {
                new FightAction(currentHero, new NPC(),  currentLocation),
             //   new WalkToAction(currentHero, currentLocation),
                new TalkToAction(currentHero, new Enemy(),  new Location()),
                new CollectAction(currentHero, new ObjectToCollect(), rnd.Next(1,4) , new Location()), // Eval Funktion WE SHOULD MOVE TO THIS NEW POSITION
                new OpenAction(currentHero, new ObjectToOpen(), currentLocation)
                };
            }
        }

        Location GetNewLocation()
        {
            return currentLocation;
        }

        Hero GetNewHero()
        {
            return new Hero();
        }

        void PickOutActions()
        {
            int size = rnd.Next(minSize, maxSize);

            for (int i = 0; i < size; i++)
            {
                int tempSel = rnd.Next(0, (avalableActions.Count));
                Thread.Sleep(30);
                selectedAction = avalableActions[tempSel];
                if (selectedAction is WalkToAction)
                {
                    SetNewLocation();
                }
                choosenActions.Add(selectedAction);

               // Console.WriteLine(choosenActions[i].OutputComposition());

                RerollActionTable(isEvil);
            }
        }
        void SetNewLocation()
        {
            currentLocation = new Location();
            WalkToAction temp = selectedAction as WalkToAction;
            temp.Location = currentLocation;
            selectedAction = temp;
        }

        public static CollectAction NewCollectAction()
        {
            return new CollectAction(currentHero, new ObjectToCollect(), rnd.Next(1, 4), currentLocation);
        }
        public static OpenAction NewOpenAction()
        {
            return new OpenAction(currentHero, new ObjectToOpen(), currentLocation);
        }

    }
}
