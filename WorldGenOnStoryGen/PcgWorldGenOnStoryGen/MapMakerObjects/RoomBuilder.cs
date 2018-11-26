using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class RoomBuilder
    {
        Tile[,] grid;
        List<Room> rooms;
        Vector2 minRoomSize, maxRoomSize;           
        Vector2 gridBounds, pos, roomSize;
        Random rnd;
        Location location;
        TileType tiletype;

        public RoomBuilder(Tile[,] grid, Vector2 minRoomSize, Vector2 maxRoomSize, Location location)
        {
            this.grid = grid;
            this.minRoomSize = minRoomSize;
            this.maxRoomSize = maxRoomSize;
            this.location = location;
            gridBounds.X = grid.GetUpperBound(1);
            gridBounds.Y = grid.GetUpperBound(0);

            InitVariables();
        }

        public RoomBuilder(Tile[,] grid, Vector2 minRoomSize, Vector2 maxRoomSize, TileType tiletype, Vector2 pos)
        {
            this.grid = grid;
            this.minRoomSize = minRoomSize;
            this.maxRoomSize = maxRoomSize;
            this.tiletype = tiletype;
            gridBounds.X = grid.GetUpperBound(1);
            gridBounds.Y = grid.GetUpperBound(0);            
            this.pos = pos;
            InitVariables();
        }

        public RoomBuilder(Tile[,] grid, Vector2 minRoomSize, Vector2 maxRoomSize, TileType tiletype, Rectangle targetArea)
        {
            this.grid = grid;
            this.minRoomSize = minRoomSize;
            this.maxRoomSize = maxRoomSize;
            this.tiletype = tiletype;
            gridBounds.X = grid.GetUpperBound(1);
            gridBounds.Y = grid.GetUpperBound(0);         
            InitVariables();
            SetPosFromArea(targetArea);
        }

        void InitVariables()
        {
            rnd = new Random();            
        }

        void SetPosFromArea(Rectangle area)
        {
            pos.X = area.X;
            pos.Y = area.Y;
        }

        void TrySize()
        {
            roomSize.X = rnd.Next((int)minRoomSize.X, (int)maxRoomSize.X);
            roomSize.Y = rnd.Next((int)minRoomSize.Y, (int)maxRoomSize.Y);
        }

        public Room GenerateRoom()
        {
            try
            {
                Vector2 selectedRoomPos;
                TrySize();
                if (pos != Vector2.Zero)
                    selectedRoomPos = pos;
                else
                    selectedRoomPos = RoomPosition(roomSize);

                List<Tile> tiles = new List<Tile>();

                for (int i = -((int)roomSize.Y / 2); i < ((int)roomSize.Y / 2); i++)
                {
                    for (int j = -((int)roomSize.X / 2); j < ((int)roomSize.X / 2); j++)
                    {
                        tiles.Add(grid[(int)selectedRoomPos.Y + i, (int)selectedRoomPos.X + j]);
                    }
                }

                bool hasFoundValidEntry = false;
                Tile entrypoint = null;
                while (!hasFoundValidEntry)
                {
                    entrypoint = SetEntrypoint(roomSize, selectedRoomPos);
                    if (EntryIsFree(entrypoint, tiles))
                        hasFoundValidEntry = true;
                }

                if (CheckTilesIsFree(tiles))
                {
                    if (location != null)
                        return new Room(tiles, location.LocationType, entrypoint);
                    else
                        return new Room(tiles, tiletype, entrypoint);
                }

                return GenerateRoom();
            }
            catch (Exception ex)
            {

            }

            return GenerateRoom();
        }

        Vector2 RoomPosition(Vector2 size)
        {
            Vector2 roomPos;
            int sizeX = (int)size.X;
            int sizeY = (int)size.Y;
            int entryOffset = 1;
            roomPos.X = rnd.Next((sizeX / 2) + entryOffset, (int)gridBounds.X - (sizeX / 2 + entryOffset));
            roomPos.Y = rnd.Next((sizeY / 2) + entryOffset, (int)gridBounds.Y - (sizeY / 2 + entryOffset));

            while (roomPos.X + (sizeX ) > gridBounds.X || roomPos.X - (sizeX ) < 0)
                roomPos.X = rnd.Next(0, (int)gridBounds.X);
            while (roomPos.Y + (sizeY) > gridBounds.Y || roomPos.Y - (sizeY) < 0)
                roomPos.Y = rnd.Next(0, (int)gridBounds.Y);
            return roomPos;
        }

        Tile SetEntrypoint(Vector2 selectedSize, Vector2 selectedRoomPos)
        {
            List<Tile> entryPoints = new List<Tile>();
            for (int i = -((int)selectedSize.Y / 2) - 1; i < ((int)selectedSize.Y / 2) + 1; i++)
            {
                for (int j = -((int)selectedSize.X / 2) - 1; j < ((int)selectedSize.X / 2) + 1; j++)
                {
                    entryPoints.Add(grid[(int)selectedRoomPos.Y + i, (int)selectedRoomPos.X + j]);
                }
            }
            return entryPoints[rnd.Next(0, entryPoints.Count)];
        }

        bool CheckTilesIsFree(List<Tile> tiles)
        {
            //freedom!
            foreach (Tile item in tiles)
            {
                if (item.GetTileType() != TileType.NONE)
                    return false;
            }
            return true;
        }
        bool EntryIsFree(Tile tile, List<Tile> tiles)
        {
            //freedom?
            if (tiles.Contains(tile))
                return false;
            return true;
        }
    }
}
