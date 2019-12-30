using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class Room
    {
      
        public List<Tile> roomTiles { get; set; }
        int indexY, indexX;
        TileType typeOfRoom;
        public Tile pathEntry { get; set; }
        Tile[,] grid;
        public Room(ref Tile[,] _grid, List<Tile> tiles, TileType typeOfRoom, Tile pathEntry)
        {
            roomTiles = tiles;            
            this.typeOfRoom = typeOfRoom;
            this.pathEntry = pathEntry;
            if(this.pathEntry.GetTileType() != TileType.NONE)
                Console.WriteLine("ROOM WITH WRONG TILETYPE CONSTRUCTED");
            grid = _grid;
            InitGlobalVariables();
            SetTileType();           
        }

        void InitGlobalVariables()
        {
            pathEntry.IsEntry = true;
        }   
        
        void SetTileType()
        {         
           
            foreach (Tile tile in roomTiles)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        if (grid[i, j].X == tile.X && grid[i, j].Y == tile.Y)
                        {
                            grid[i,j].SetTileType(typeOfRoom);
                            grid[i, j].IsWalkable = false;
                            grid[i, j].IsBranchTile = false;
                        }
                }
                }
                              
            }

        }
    }
}
