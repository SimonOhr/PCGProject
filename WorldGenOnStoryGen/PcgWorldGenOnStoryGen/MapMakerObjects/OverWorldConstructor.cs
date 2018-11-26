using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen.MapMakerObjects
{
    class OverWorldConstructor
    {
        Tile[,] grid;
        List<Room> rooms;
        Random rnd;
        Vector2 gridBounds;
        List<Rectangle> subdivisions;        
        public OverWorldConstructor(Tile[,] _grid)
        {
            grid = _grid;
            InitVariables();
        }

        void InitVariables()
        {
            rnd = new Random();
            gridBounds.Y = grid.GetUpperBound(0);
            gridBounds.X = grid.GetUpperBound(1);
            rooms = new List<Room>();
            subdivisions.Add(new Rectangle(0, 0, (int)gridBounds.X, (int)gridBounds.Y));
        }

        public List<Room> CreateOverWorld(int iterations)
        {
            subdivisions = DivisionIterations(iterations);
            return rooms;
        }

        List<Rectangle> DivisionIterations(int iterations)
        {
            List<Rectangle> tempRecs = new List<Rectangle>();
            tempRecs.Add(subdivisions.First());
            for (int i = 0; i < iterations; i++)
            {
                int x = tempRecs[i].X;
                int y = tempRecs[i].Y;                
            }
            return null;
        }

        Vector2 RoomPosition(int size)
        {
            Vector2 roomPos;
            int entryOffset = 1;
            roomPos.X = rnd.Next((size / 2) + entryOffset, (int)gridBounds.X - (size / 2 + entryOffset));
            roomPos.Y = rnd.Next((size / 2) + entryOffset, (int)gridBounds.Y - (size / 2 + entryOffset));

            while (roomPos.X + (size / 2) > gridBounds.X || roomPos.X - (size / 2) < 0)
                roomPos.X = rnd.Next(0, (int)gridBounds.X);
            while (roomPos.Y + (size / 2) > gridBounds.Y || roomPos.Y - (size / 2) < 0)
                roomPos.Y = rnd.Next(0, (int)gridBounds.Y);
            return roomPos;
        }
    }
}
