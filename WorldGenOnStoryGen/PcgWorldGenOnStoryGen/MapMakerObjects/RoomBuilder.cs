using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PcgWorldGenOnStoryGen
{
    internal class RoomBuilder
    {
        private Tile[,] grid;
        private readonly List<Room> rooms;
        private Vector2 minRoomSize, maxRoomSize;
        private Vector2 gridBounds, pos, roomSize;
        private Random rnd;
        private Location location;
        private readonly TileType tiletype;
        private int entryOffset;
        public static int SuccessCount;
        public static int NumberOfRooms;

        public RoomBuilder(ref Tile[,] grid, Vector2 minRoomSize, Vector2 maxRoomSize, Location location)
        {
            this.grid = grid;
            this.minRoomSize = minRoomSize;
            this.maxRoomSize = maxRoomSize;
            this.location = location;
            gridBounds.X = grid.GetUpperBound(1);
            gridBounds.Y = grid.GetUpperBound(0);

            InitVariables();
        }
        /// <summary>
        /// currently not in use
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="minRoomSize"></param>
        /// <param name="maxRoomSize"></param>
        /// <param name="tiletype"></param>
        /// <param name="pos"></param>
        public RoomBuilder(ref Tile[,] grid, Vector2 minRoomSize, Vector2 maxRoomSize, TileType tiletype, Vector2 pos)
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

        //public RoomBuilder(Tile[,] grid, Vector2 minRoomSize, Vector2 maxRoomSize, TileType tiletype, Rectangle targetArea)
        //{
        //    this.grid = grid;
        //    this.minRoomSize = minRoomSize;
        //    this.maxRoomSize = maxRoomSize;
        //    this.tiletype = tiletype;
        //    gridBounds.X = grid.GetUpperBound(1);
        //    gridBounds.Y = grid.GetUpperBound(0);         
        //    InitVariables();
        //    SetPosFromArea(targetArea);
        //}

        private void InitVariables()
        {
            rnd = new Random();
            entryOffset = 1;
        }

        private void SetPosFromArea(Rectangle area)
        {
            pos.X = area.X;
            pos.Y = area.Y;
        }

        private void TrySize()
        {
            roomSize.X = rnd.Next((int)minRoomSize.X, (int)maxRoomSize.X);
            roomSize.Y = rnd.Next((int)minRoomSize.Y, (int)maxRoomSize.Y);
        }

        public Room GenerateRoom()
        {
            //try
            //{
            Vector2 selectedRoomPos;
            TrySize();
            if (pos != Vector2.Zero)
                selectedRoomPos = pos;
            else
                selectedRoomPos = RoomPosition(roomSize);

            List<Tile> roomGrid = new List<Tile>();


            for (int i = -((int)roomSize.Y / 2) + -entryOffset; i < ((int)roomSize.Y / 2); i++)
            {
                for (int j = -((int)roomSize.X / 2) + -entryOffset; j < ((int)roomSize.X / 2); j++)
                {                    
                    roomGrid.Add(grid[(int)selectedRoomPos.Y + i, (int)selectedRoomPos.X + j]);

                }
            }
          

            bool hasFoundValidEntry = false;
            Tile entryPoint = null;
            int errorCount = 0;

            while (!hasFoundValidEntry)
            {
                entryPoint = SetEntrypoint(roomSize, selectedRoomPos);
                //if (EntryIsFree(entrypoint, tiles))
                    //
                //else
                    Console.WriteLine("No Valid Entry Found: {0}", errorCount++);
                if (entryPoint == null || errorCount > 10)
                {
                    Console.WriteLine("No Valid Entry Found");
                    Console.WriteLine("Rebuilding Room...");
                    return GenerateRoom();
                }
                hasFoundValidEntry = true;
            }

            if (CheckTilesAreFree(roomGrid))
            {
                if (location != null)
                {
                    SuccessCount++;
                    Console.WriteLine("Number of seccessfully build rooms: {0} / {1}", SuccessCount, NumberOfRooms);

                    return new Room(ref grid, roomGrid, location.LocationType, entryPoint);
                }
                // else
                //   return new Room(ref grid, tiles, tiletype, entrypoint);
                throw new Exception("Room Location is NULL");
            }

            return GenerateRoom();
            // }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    throw ex;
            //}                        
        }

        private Vector2 RoomPosition(Vector2 size)
        {
            Vector2 roomPos;
            int sizeX = (int)size.X;
            int sizeY = (int)size.Y;
            int entryOffset = 1;
            roomPos.X = rnd.Next((sizeX / 2) + entryOffset, (int)gridBounds.X - (sizeX / 2 + entryOffset)); //TODO why / 2? on minimum
            roomPos.Y = rnd.Next((sizeY / 2) + entryOffset, (int)gridBounds.Y - (sizeY / 2 + entryOffset));  // TODO why / 2?

            while (roomPos.X + (sizeX / 2) + entryOffset > gridBounds.X || roomPos.X - (sizeX / 2 + entryOffset) < 0)
                roomPos.X = rnd.Next(0, (int)gridBounds.X);
            while (roomPos.Y + (sizeY / 2) + entryOffset > gridBounds.Y || roomPos.Y - (sizeY / 2 + entryOffset) < 0)
                roomPos.Y = rnd.Next(0, (int)gridBounds.Y);

            return roomPos;
        }

        private Tile SetEntrypoint(Vector2 selectedSize, Vector2 selectedRoomPos)
        {
            List<Tile> entryPoints = new List<Tile>();
            try
            {
                for (int i = -((int)selectedSize.Y / 2) + (-entryOffset); i < ((int)selectedSize.Y / 2 + (entryOffset)); i++)
                {
                    for (int j = -((int)selectedSize.X / 2 + (-entryOffset)); j < ((int)selectedSize.X / 2 + (entryOffset)); j++)
                    {
                        if (i == -((int)selectedSize.Y / 2) + (-entryOffset) || i == ((int)selectedSize.Y / 2 + (entryOffset)))
                        {
                            entryPoints.Add(grid[(int)selectedRoomPos.Y + i, (int)selectedRoomPos.X + j]);
                        }
                        else if (j == -((int)selectedSize.X / 2 + (-entryOffset)) || j == ((int)selectedSize.X / 2 + (entryOffset)))
                        {
                            entryPoints.Add(grid[(int)selectedRoomPos.Y + i, (int)selectedRoomPos.X + j]);
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GOTCHA!");
            }
            
            Tile[] entryPointsArray = new Tile[entryPoints.Count];
            entryPointsArray = FisherYates.Shuffle<Tile>(new Random(), entryPoints.ToArray());
            Queue<Tile> popStack = new Queue<Tile>();
            int iterator = 0; /*rnd.Next(0,entryPoints.Count-1); */
            popStack.Enqueue(entryPointsArray[iterator]);
            Tile entryPoint = popStack.Dequeue();
            while (entryPoint.GetTileType() != TileType.NONE) // Check if new room instance completed, or no tile change
            {
                if (iterator < entryPointsArray.Length - 1)
                {
                    iterator++;
                }
                else
                {
                    return null;
                }
                popStack.Enqueue(entryPointsArray[iterator]);
                entryPoint = popStack.Dequeue();
            }

            return entryPoint;
        }

        private bool CheckTilesAreFree(List<Tile> tiles)
        {
            //freedom!
            foreach (Tile item in tiles)
            {
                if (item.GetTileType() != TileType.NONE)
                    return false;
            }
            return true;
        }

        //private bool EntryIsFree(Tile tile, List<Tile> tiles)
        //{
        //    //freedom?
        //    if (tiles.Contains(tile) && tile.GetTileType() == TileType.NONE)
        //        return true;
        //    return false;
        //}
    }
}
