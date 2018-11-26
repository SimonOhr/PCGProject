using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class QuestEnemiesEvaluation
    {
        public QuestEnemiesEvaluation(List<DebugQuest> quests)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                GetFightTypes(quests[i]);
            }
        }

        void GetFightTypes(DebugQuest quest)
        {
            List<FightAction> goodFights = new List<FightAction>();
            List<FightAction> evilFights = new List<FightAction>();
            foreach (Action item in quest.actionArray)
            {
                if (item is FightAction)
                {
                    if (quest.IsEvil)
                    {
                        evilFights.Add(item as FightAction);
                    }
                    else
                    {
                        goodFights.Add(item as FightAction);
                    }
                }
            }
            ScoreFightsAtLocations(evilFights, true, quest);
            ScoreFightsAtLocations(goodFights, false, quest);
        }

        void ScoreFightsAtLocations(List<FightAction> fights, bool isEvil, DebugQuest quest)
        {
            if (isEvil)
            {
                foreach (FightAction item in fights)
                {
                    if (item.Location.outputComposition == "Castle")
                    {
                        if (item.TypeOfEnemy == "Princess")
                        {
                            quest.Fitness += 20;
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Knight")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Friend")
                        {
                            quest.Fitness += 10;
                        }
                    }
                    else if (item.Location.outputComposition == "Town")
                    {
                        if (item.TypeOfEnemy == "Princess")
                        {
                            quest.Fitness -= 20;
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Knight")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Friend")
                        {
                            
                        }
                    }
                    else if (item.Location.outputComposition == "Dungeon")
                    {
                        if (item.TypeOfEnemy == "Princess")
                        {
                            quest.Fitness -= 20;
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Knight")
                        {
                           
                        }
                        else if (item.TypeOfEnemy == "Friend")
                        {
                            quest.Fitness -= 20;
                        }
                    }
                    else if (item.Location.outputComposition == "Wild")
                    {
                        if (item.TypeOfEnemy == "Princess")
                        {
                            quest.Fitness -= 20;
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Knight")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Friend")
                        {
                           
                        }
                    }
                }
            }
            else
            {
                foreach (FightAction item in fights)
                {
                    if (item.Location.outputComposition == "Castle")
                    {
                        if (item.TypeOfEnemy == "Boss")
                        {
                            
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Skeleton")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Zombie")
                        {
                            quest.Fitness += 10;
                        }
                    }
                    else if (item.Location.outputComposition == "Town")
                    {
                        if (item.TypeOfEnemy == "Boss")
                        {
                            quest.Fitness -= 20;
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Skeleton")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Zombie")
                        {
                            quest.Fitness += 10;
                        }
                    }
                    else if (item.Location.outputComposition == "Dungeon")
                    {
                        if (item.TypeOfEnemy == "Boss")
                        {
                            quest.Fitness += 20;
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Skeleton")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Zombie")
                        {
                            quest.Fitness += 10;
                        }
                    }
                    else if (item.Location.outputComposition == "Wild")
                    {
                        if (item.TypeOfEnemy == "Boss")
                        {                           
                            quest.PrimaryTargetCounter++;
                        }
                        else if (item.TypeOfEnemy == "Skeleton")
                        {
                            quest.Fitness += 10;
                        }
                        else if (item.TypeOfEnemy == "Zombie")
                        {
                            quest.Fitness += 10;
                        }
                    }
                }
            }
            if (quest.PrimaryTargetCounter > 0)
                quest.Fitness += 10 + (quest.PrimaryTargetCounter * -10);
            quest.PrimaryTargetCounter = 0;
        }
    }
}
