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
        public Room(List<Tile> tiles, TileType typeOfRoom, Tile pathEntry)
        {
            roomTiles = tiles;            
            this.typeOfRoom = typeOfRoom;
            this.pathEntry = pathEntry;
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
                tile.SetTileType(typeOfRoom);
                tile.IsWalkable = false;
                tile.IsBranchTile = false;                
            }
        }
    }
}
