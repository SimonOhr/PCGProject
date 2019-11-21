using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class BoardManager
    {
        Tile[,] tiles;
        TileType selectedType;
        Random rnd;
        GameWindow Window;
        Texture2D spriteMap;
        ActionGen gen;
        List<DebugQuest> quests;
        MapMaker mapMaker;
        QuestsEval questsEval;



        int tileSize, tileTypeCount, sizeOfQuestPool;

        public BoardManager(GameWindow Window, Texture2D spriteMap, ref List<Map> maps)
        {
            this.Window = Window;
            this.spriteMap = spriteMap;

            InitGlobalVariables();

            //mapMaker.ExctractLocation(DQuest);
            
            GetQuests();
            EvalQuests();
            MakeMap(ref maps);
        }

        void InitGlobalVariables()
        {
            tileSize = 32;
            tileTypeCount = 5;
            sizeOfQuestPool = 10;
            rnd = new Random();
            gen = new ActionGen();
            mapMaker = new MapMaker(this, Window, spriteMap);
            quests = new List<DebugQuest>();
        }

        void GetQuests()
        {
            Console.WriteLine("Constrcuting new quests...");
            for (int i = 0; i < sizeOfQuestPool; i++)
            {
                quests.Add(gen.GetNewQuest()); //ToDo evaluation, selection map construciton save to mapobject, gui PRESENTATION       
                Console.WriteLine("{0}/{1}", quests.Count, sizeOfQuestPool);
            }
        }

        void EvalQuests()
        {
            Console.WriteLine("Evaluation quests...");
            questsEval = new QuestsEval(ref quests);
            questsEval.FlowHandler();
        }

        void MakeMap(ref List<Map> maps)
        {
            Console.WriteLine("Constructing Maps...");
            for (int i = 0; i < quests.Count; i++)
            {
                mapMaker.Clear();
                mapMaker.CreateGrid();
                CopyNewGrid();
                mapMaker.CreateRooms(quests[i]);
                mapMaker.RunMiners();
                foreach (Tile tile in mapMaker.GetMapInfo().tileMap)
                {
                    if (tile.GetTileType() == TileType.NONE)
                        tile.SetTileType(TileType.WOOD);
                }
                maps.Add(mapMaker.GetMapInfo());
                maps[i].PrintMapInfo();                
                
            }
        }
        void CopyNewGrid()
        {
            tiles = mapMaker.Tiles;
            for (int i = 0; i < mapMaker.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < mapMaker.Tiles.GetLength(1); j++)
                {
                    tiles[i, j] = mapMaker.Tiles[i, j];
                }
            }
        }

        //public Tile[,] GetTileGrid()
        //{
        //    tiles = new Tile[Window.ClientBounds.Height, Window.ClientBounds.Height];

        //    for (int i = 0; i < tiles.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < tiles.GetLength(1); j++)
        //        {
        //            Vector2 tempPos = new Vector2(j * tileSize, i * tileSize);

        //            selectedType = (TileType)rnd.Next(0, tileTypeCount); // DEBUG
        //            tiles[i, j] = new Tile(spriteMap, tempPos);
        //        }
        //    }
        //    return tiles;
        //}

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j].Draw(sb);
                }
            }
        }
    }
}
