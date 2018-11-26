using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class QuestVarietyEvaluation
    {
        int scorePerVariation;
        int pentaltyOnMultiple;

        /// <summary>
        /// rewards variation and punishes stagnation
        /// </summary>
        /// <param name="quests"></param>
        public QuestVarietyEvaluation(List<DebugQuest> quests)
        {
            scorePerVariation = 50;
            pentaltyOnMultiple = -5;

            for (int i = 0; i < quests.Count; i++)
            {
                GetActions(quests[i]);
            }
        }
        /// <summary>
        /// creates a dictionary and adds each action, then iterates over the keyvaluepairs and rewards on variation and punishes on stagnation
        /// </summary>
        /// <param name="quest"></param>
        void GetActions(DebugQuest quest)
        {
            Dictionary<Action, int> actionCounter = new Dictionary<Action, int>();

            List<FightAction> fights = new List<FightAction>();
            List<WalkToAction> walks = new List<WalkToAction>();
            List<TalkToAction> talks = new List<TalkToAction>();
            List<CollectAction> collects = new List<CollectAction>();
            // List<OpenAction> opens = new List<OpenAction>();

            foreach (Action item in quest.actionArray)
            {
                if (item is FightAction)
                {
                    fights.Add(item as FightAction);
                }
                else if (item is WalkToAction)
                {
                    walks.Add(item as WalkToAction);
                }

                else if (item is TalkToAction)
                {
                    talks.Add(item as TalkToAction);
                }
                else if(item is CollectAction)
                {
                    collects.Add(item as CollectAction);
                }

                // else if (item is OpenAction)
                //  opens.Add(item as OpenAction);
            }
            ScoreVariety(fights, quest);
            ScoreVariety(walks, quest);
            ScoreVariety(talks, quest);
            ScoreVariety(collects, quest);
            GetInstances(quest);

        }
        void ScoreVariety<T>(List<T> actions, DebugQuest quest)
        {
            int it = 0;
            quest.Fitness += scorePerVariation;
            foreach (T item in actions)
            {
                it++;
                if (item is FightAction)
                    continue;
                else if (it > 1)
                {
                    quest.Fitness += pentaltyOnMultiple; // nullifies setback on first instance
                }
            }
        }
        /// <summary>
        /// rewards diversity within the actions i.e Fight Skeleton-> Fight Zombie would be more interesting than Fight Skeleton-> Fight Skeleton atleast in a generic sense
        /// </summary>
        void GetInstances(DebugQuest quest) // TODO find way to save actions to the dictionary whch would make this function nicer
        {
            List<string> enemies = new List<string>();
            List<string> objects = new List<string>();
            List<string> locations = new List<string>();
            List<string> npc = new List<string>();
            for (int i = 0; i < quest.actionArray.Length; i++)
            {
                if (quest.actionArray[i] is FightAction)
                {
                    enemies.Add(quest.actionArray[i].TypeOfEnemy);
                }
                else if (quest.actionArray[i] is CollectAction)
                {
                    objects.Add(quest.actionArray[i].TypeOfObject);
                }
                else if (quest.actionArray[i] is WalkToAction)
                {
                    locations.Add(quest.actionArray[i].TypeOfLocation);
                }
                else if (quest.actionArray[i] is TalkToAction)
                {
                    npc.Add(quest.actionArray[i].TypeOfFriend);
                }
            }
            ScoreDiversity(objects, quest);
            ScoreDiversity(npc, quest);
            ScoreDiversity(locations, quest);
            ScoreDiversity(enemies, quest);
        }

        void ScoreDiversity(List<string> collection, DebugQuest quest)
        {
            int counter = 0;
            for (int i = 0; i < collection.Count-1; i++)
            {
                for (int j = i+1; j < collection.Count; j++)
                {
                    if(i != j)
                    {
                        if(collection[i] == collection[j])
                        {
                            quest.Fitness -= 5;
                            counter++;
                            if (counter > 3)
                                quest.Fitness -= 10;
                        }
                    }
                }
            }
        }
    }
}
