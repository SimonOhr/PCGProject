using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class PathingMiner
    {
        AstarPathing pathing;
        List<Room> rooms;
        List<Tile> trail;
        public PathingMiner(Tile[,] grid, List<Room> rooms)
        {
            pathing = new AstarPathing(grid);
            this.rooms = rooms;
            trail = new List<Tile>();
            BuildPaths();
        }

        void BuildPaths()
        {           
            try
            {
                for (int i = 0; i + 1 < rooms.Count; i++)
                {
                    trail.AddRange(pathing.FindPath(rooms[i].pathEntry, rooms[i + 1].pathEntry));
                }

                foreach (Tile item in trail)
                {
                    item.SetTileType(TileType.MINER);                    
                }
            }
            catch(Exception ex)
            {
                //BuildPaths();
            }           
        }

        public List<Tile> GetTrail()
        {
            return trail;
        }
    }
}
