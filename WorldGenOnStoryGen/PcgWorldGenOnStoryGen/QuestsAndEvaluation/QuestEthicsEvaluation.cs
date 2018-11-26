using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class QuestEthicsEvaluation
    {
        public QuestEthicsEvaluation(List<DebugQuest> quests)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                CheckEthics(quests[i]);
            }
        }

        void CheckEthics(DebugQuest quest)
        {
            int value = 0;
            foreach (Action item in quest.actionArray)
            {
                value += item.EthicsValue;
            }
            SetEthicsValue(value, quest);
        }

        void SetEthicsValue(int value, DebugQuest quest)
        {
            quest.EthicsValue = value;
        }
    }
}
