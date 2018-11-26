using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class Extractor
    {
        List<Tile> islands;
        Tile[,] Tiles;
        int indexY, indexX, tileSize, previousQuestMarker;
        Random rnd;

        public Extractor(MapMaker mk)
        {
            InitGlobalValues();
            Tiles = mk.Tiles;
        }

        void InitGlobalValues()
        {
            islands = new List<Tile>();
            rnd = new Random();
            tileSize = 32;
        }

        public void InsertIntoPath(DebugQuest quest, List<Tile> validPath) // rewrite to handle positions by miner, set ENd of path to island
        {           
            for (int i = 0; i < validPath.Count; i++)
            {
                if (validPath[i].IsEndOfPath)
                {
                    if (SpawnChance(5))
                        previousQuestMarker = ExtractQuestLocations(quest, (previousQuestMarker), validPath[i]);
                }
            }
        }

        bool SpawnChance(int ratio)
        {
            if (rnd.Next(0, ratio) == 0)
            {
                return true;
            }
            return false;
        }

        int ExtractQuestLocations(DebugQuest quest, int _previous, Tile tile)
        {

            for (int i = _previous; i < quest.actionArray.Length; i++)
            {
                if (quest.actionArray[i].Location.outputComposition == "Castle")
                {
                    tile.SetTileType(TileType.CASTLE);
                    //islands.Add(Tiles[indexX, indexY]);
                    return ++i;
                }
                else if (quest.actionArray[i].Location.outputComposition == "Dungeon")
                {
                    tile.SetTileType(TileType.DUNGEON);
                    //islands.Add(Tiles[indexX, indexY]);
                    return ++i;
                }
                else if (quest.actionArray[i].Location.outputComposition == "Town")
                {
                    tile.SetTileType(TileType.TOWN);
                    //  islands.Add(Tiles[indexX, indexY]);
                    return ++i;
                }
                else if (quest.actionArray[i].Location.outputComposition == "Wild")
                {
                    tile.SetTileType(TileType.GRASS);
                    // islands.Add(Tiles[indexX, indexY]);
                    return ++i;
                }
            }
            return quest.actionArray.Length;
        }       
    }
}
