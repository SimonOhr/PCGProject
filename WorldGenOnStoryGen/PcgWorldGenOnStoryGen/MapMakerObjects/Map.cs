using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class Map
    {
        public Tile[,] tileMap { get; set; }
        DebugQuest quest;
        public Map(Tile[,] _tileMap, DebugQuest _quest)
        {
            tileMap = new Tile[_tileMap.GetLength(1), _tileMap.GetLength(0)];
            quest = _quest;
            Array.Copy(_tileMap, tileMap, _tileMap.Length);
        }

        public void PrintMapInfo()
        {
            string mapInfo = "";
            mapInfo += $"MapInfo: size->{quest.actionArray.Length}, ";
            for (int i = 0; i < quest.actionArray.Length; i++)
            {
                mapInfo += quest.actionArray[i] + ", ";
            }
            mapInfo += "End of Map";
            Console.WriteLine(mapInfo);
        }
    }
}
