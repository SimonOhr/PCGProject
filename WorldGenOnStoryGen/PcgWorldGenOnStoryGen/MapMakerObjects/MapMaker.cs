using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Threading;

namespace PcgWorldGenOnStoryGen
{
    class MapMaker
    {
        DebugQuest quest;
        public Tile[,] Tiles;
        public List<Tile> Trail { get; set; }
        List<Tile> islandCandidates;
        public Queue<Tile> IslandQueue { get; set; }
        BoardManager bm;
        RoomBuilder roomBuilder;       
        PathingMiner pathMiner;
        GameWindow Window;
        Texture2D spriteMap;
        List<Miner> miners;
        Extractor extractor;
        List<Room> rooms;
        Random rnd;        

        int indexY, indexX, tileSize;
        public MapMaker(BoardManager bm, GameWindow Window, Texture2D spriteMap)
        {
            this.Window = Window;
            this.spriteMap = spriteMap;
            this.bm = bm;
            InitValues();
         //   CreateGrid();
            InitGlobalObject();
        }

        void InitValues()
        {
            tileSize = 32;
            rnd = new Random();
            islandCandidates = new List<Tile>();
            rooms = new List<Room>();
        }

        void InitGlobalObject()
        {
            Trail = new List<Tile>();
            miners = new List<Miner>();          
            extractor = new Extractor(this);
        }

        public void CreateGrid()
        {
            Tiles = new Tile[Game1.ScreenHeight / tileSize, Game1.ScreenWidth / tileSize];

            int[] tempWeights = new int[8];
            for (int k = 0; k < tempWeights.Length; k++)
            {
                tempWeights[k] = k+4;
            }

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Vector2 tempPos = new Vector2(j * tileSize, i * tileSize);
                    FisherYates.Shuffle<int>(new Random(), tempWeights);
                    int tempWeight = tempWeights[0];
                    Tiles[i, j] = new Tile(spriteMap, tempPos, j, i, tempWeight);

                  //  Console.WriteLine($"Tile[i,j] = {Tiles[i, j]} pos info = {Tiles[i, j].Dest.Location} X = {Tiles[i, j].X} ({Tiles[i, j].Dest.Location.X / tileSize}) Y = {Tiles[i, j].Y} ({Tiles[i, j].Dest.Location.Y / tileSize})");
                }
            }
        }

        public void CreateRooms(DebugQuest _quest)
        {
            quest = _quest;
            RoomBuilder.SuccessCount = 0;
            RoomBuilder.NumberOfRooms = quest.actionArray.Length;
                for (int j = 0; j < quest.actionArray.Length; j++)
                {
                    rooms.Add(new RoomBuilder(ref Tiles, new Vector2(2, 2), new Vector2(6, 6), quest.actionArray[j].Location).GenerateRoom());                    
                }

            // BSPTree BSPTree = new BSPTree(quest);
            // BSPTree.MakeTree(Tiles, new Vector2(2, 2), new Vector2(5, 5), 1);          
        }

        public void RunMiners()
        {
            pathMiner = new PathingMiner(ref Tiles, rooms);

            Trail.AddRange(pathMiner.GetTrail());

            //foreach (Tile tile in Trail)
            //{
            //    tile.RollBranchTile(rnd.Next(0, Trail.Count));
            //    if (tile.IsBranchTile)
            //        miners.Add(new Miner(this, tile));
            //}
            //foreach (Miner miner in miners)
            //{
            //    miner.StartMiner(this);
            //    Trail.AddRange(miner.GetTrail());
            //}
            //foreach (Tile tile in Trail)
            //{
            //    if (tile.IsEndOfPath)
            //    {
            //        rooms.Add(new RoomBuilder(Tiles, 0, 6, TileType.GRASS, new Vector2(tile.X, tile.Y)).GenerateRoom());
            //    }
            //}           
        }

        public Map GetMapInfo()
        {
            return new Map(Tiles, quest);
        }

        public void Clear()
        {
            rooms.Clear();
        }

        //public void InsertIntoPathIslands(DebugQuest quest)
        //{
        //    //  mapInfo.InsertIntoPath(quest, Trail);
        //}
    }
}
