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
        public PathingMiner(ref Tile[,] grid, List<Room> rooms)
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
                    
                    foreach (Tile tile in trail)
                    {
                        tile.SetWeight(0);
                    }                   
                    if(trail == null)
                    {
                        throw new Exception();
                    }
                }

                int iter = 0;
                foreach (Tile item in trail)
                {
                    item.SetTileType(TileType.MINER);
                    iter++;
                    if (iter == trail.Count)
                    {
                        int stop = 0;
                    }
                       
                }
                if(trail.Count <= rooms.Count)
                {
                    throw new Exception();
                }
            }
            catch(Exception ex)
            {
                //BuildPaths();
                Console.WriteLine(ex);
                Console.WriteLine(ex.ToString());
            }           
        }

        public List<Tile> GetTrail()
        {
            List<Tile> temp = trail;           
            return temp;
        }
    }
}
