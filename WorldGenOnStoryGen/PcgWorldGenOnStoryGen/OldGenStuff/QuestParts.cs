using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public static class QuestParts
    {
        static string[] parts = new string[20];

        static string[] compObjects = new string[5];
        static string[] compActions = new string[7];
        static string[] compNPCs = new string[3];
        static string[] compEnemies = new string[3];
        static string[] compLocations = new string[2];

        public static void BuildParts() //old system
        {
            parts[0] = "Key";
            parts[1] = "Fight";
            parts[2] = "Door";
            parts[3] = "Treasure";
            parts[4] = "Skeleton";
            parts[5] = "Boss";
            parts[6] = "Talk To";
            parts[7] = "Knight";
            parts[8] = "Walk To";
            parts[9] = "Dungeon";
            parts[10] = "Zombie";
            parts[11] = "Find";
            parts[12] = "Collect";
            parts[13] = "Friend";
            parts[14] = "Open";
            parts[15] = "Big Door";
            parts[16] = "Big Key";
            parts[17] = "Princess";
            parts[18] = "Rescue";
            parts[19] = "Castle";
        }
        //public static void BuildParts()
        //{
        //    CompNPCsAssembly();
        //    CompActionsAssembly();
        //    CompNPCsAssembly();
        //    CompEnemiesAssembly();
        //    CompLocationsAssembly();

        //    parts[0] = compObjects;
        //    parts[1] = compActions;
        //    parts[2] = compNPCs;
        //    parts[3] = compEnemies;
        //    parts[4] = compLocations;
        //}
        static void CompObjectsAssembly()
        {            
            compObjects[0] = "Key";
            compObjects[1] = "Door";
            compObjects[2] = "Treasure";
            compObjects[3] = "Big Key";
            compObjects[4] = "Big Door";           
        }

        static void CompActionsAssembly()
        {
            compActions[0] = "Fight";
            compActions[1] = "Talk To";
            compActions[2] = "Walk To";
            compActions[3] = "Rescue";
            compActions[4] = "Find";
            compActions[5] = "Collect";
            compActions[6] = "Open";
        }

        static void CompNPCsAssembly()
        {
            compNPCs[0] = "Princess";
            compNPCs[1] = "Knight";
            compNPCs[2] = "Friend";
        }
       
        static void CompEnemiesAssembly()
        {
            compEnemies[0] = "Boss";
            compEnemies[1] = "Skeleton";
            compEnemies[2] = "Zombie";
        }

        static void CompLocationsAssembly()
        {
            compLocations[0] = "Dungeon";
            compLocations[1] = "Castle";
        }
        public static string GetQuestParts(int target)
        {
            //string[] parts = new string[20];

            //parts[0] = "Key";
            //parts[1] = "Fight";
            //parts[2] = "Door";
            //parts[3] = "Treasure";
            //parts[4] = "Princess";
            //parts[5] = "Boss";
            //parts[6] = "Talk To";
            //parts[7] = "Knight";
            //parts[8] = "Walk To";
            //parts[9] = "Dungeon";
            //parts[10] = "Knight";
            //parts[11] = "Rescue";
            //parts[12] = "Find";
            //parts[13] = "Collect";
            //parts[14] = "Friend";
            //parts[15] = "Open";
            //parts[16] = "Big Door";
            //parts[17] = "Big Key";
            //parts[18] = "Skeleton";
            //parts[19] = "Zombie";



            return parts[target];
        }

        public static string[] GetQuestParts()
        {
            //string[] parts = new string[20];

            //parts[0] = "Key";
            //parts[1] = "Fight";
            //parts[2] = "Door";
            //parts[3] = "Treasure";
            //parts[4] = "Princess";
            //parts[5] = "Boss";
            //parts[6] = "Talk To";
            //parts[7] = "Knight";
            //parts[8] = "Walk To";
            //parts[9] = "Dungeon";
            //parts[10] = "Knight";
            //parts[11] = "Rescue";
            //parts[12] = "Find";
            //parts[13] = "Collect";
            //parts[14] = "Friend";
            //parts[15] = "Open";
            //parts[16] = "Big Door";
            //parts[17] = "Big Key";
            //parts[18] = "Skeleton";
            //parts[19] = "Zombie";

            return parts;
        }
    }
}
