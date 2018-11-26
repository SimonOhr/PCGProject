using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public static class FileManipulator
    {
        public static void WriteToFile(Quest[] quests, bool showNonPlayable)
        {
            System.IO.File.Delete(@"C:/Users/Simon/Desktop/SkolProjekt/År 3/PCG/PCGAssign1QuestGenerator/PCGAssign1QuestGenerator/bin/Debug/GeneratedQuests.txt)");
            for (int i = 0; i < quests.Length; i++)
            {
                if (showNonPlayable)
                {
                    string toFileString = "";
                    for (int j = 0; j < quests[i].QuestComposition.Length; j++)
                    {
                        toFileString += quests[i].QuestComposition[j];
                        toFileString += "->";
                    }
                    toFileString += $"{quests[i].Fitness} is playable:{quests[i].IsPlayable}";
                    toFileString += "\r\n\r\n";
                    System.IO.File.AppendAllText(@"C:/Users/Simon/Desktop/SkolProjekt/År 3/PCG/PCGAssign1QuestGenerator/PCGAssign1QuestGenerator/bin/Debug/GeneratedQuests.txt)", toFileString);
                }
                else
                {
                    if (quests[i].IsPlayable)
                    {
                        string toFileString = "";
                        for (int j = 0; j < quests[i].QuestComposition.Length; j++)
                        {
                            toFileString += quests[i].QuestComposition[j];
                            toFileString += "->";
                        }
                        toFileString += $"{quests[i].Fitness} is playable:{quests[i].IsPlayable}";
                        toFileString += "\r\n\r\n";
                        System.IO.File.AppendAllText(@"C:/Users/Simon/Desktop/SkolProjekt/År 3/PCG/PCGAssign1QuestGenerator/PCGAssign1QuestGenerator/bin/Debug/GeneratedQuests.txt)", toFileString);
                    }
                }
            }
        }
    }
}
