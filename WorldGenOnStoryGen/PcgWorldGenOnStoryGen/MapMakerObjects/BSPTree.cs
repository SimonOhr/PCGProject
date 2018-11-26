using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class BSPTree
    {
        public List<Node> tree = new List<Node>();
        Node root;
        AstarPathing pathing;
        public DebugQuest Quest { get; set; }
        public BSPTree(DebugQuest quest) { Quest = quest; }

        public void MakeTree(Tile[,] grid, Vector2 minArea, Vector2 maxArea, int iterations)
        {
            Rectangle rec = new Rectangle(0, 0, grid.GetLength(1), grid.GetLength(0));
            root = new Node(rec, ref grid, minArea, maxArea, 0, iterations, Quest);
            //   root.LinkToParent(pathing);
        }
    }
    class Node
    {
        Random rnd = new Random();
        Rectangle area, childARec, childBRec;
        Vector2 minSize, maxSize;
        List<Node> children = new List<Node>();
        Node parent;
        RoomBuilder roomBuilder;
        DebugQuest quest;

        public Vector2 QuorridorPos { get; private set; }

        public Node(Rectangle rec, ref Tile[,] grid, Vector2 minSize, Vector2 maxSize, int currentIteration, int maxIterations, DebugQuest quest, Node parent = null)
        {
            area = rec;
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.quest = quest;
            if (currentIteration < maxIterations)
            {
                MakeChild(ref grid, currentIteration, maxIterations);
            }
            MakeRoom(ref grid);
            SetQuorridorPosition();

        }
        void MakeChild(ref Tile[,] grid, int currentIteration, int maxIterations)
        {
            childARec = area;
            childBRec = area;

            SubDivide(ref childARec, ref childBRec);


            currentIteration++;
            if (childARec.Width >= minSize.X && childARec.Height >= minSize.Y)
            {
               // if (childARec.Width <= maxSize.X && childARec.Height <= maxSize.Y)
                    children.Add(new Node(childARec, ref grid, minSize, maxSize, currentIteration, maxIterations, quest, this));
            }

            if (childBRec.Width >= minSize.X && childBRec.Height >= minSize.Y)
            {
                //if (childBRec.Width <= maxSize.X && childBRec.Height <= maxSize.Y)
                    children.Add(new Node(childBRec, ref grid, minSize, maxSize, currentIteration, maxIterations, quest, this));
            }
        }

        void SubDivide(ref Rectangle childA, ref Rectangle childB)
        {
            int randVert = rnd.Next(0, 2);

            if (randVert > 0)
            {
                //tempArea.X /= 2;
                int randPos = rnd.Next((int)minSize.X, childA.Width);
                childA.Width = randPos;

                childB.X = childA.Width;
                childB.Width = area.Width - childA.Width; //DODO investigate if smaller than minimum size
            }
            else
            {
                // tempArea.Y /= 2;
                int randPos = rnd.Next((int)minSize.Y, childA.Height);
                childA.Height = randPos;

                childB.Y = childA.Height;
                childB.Height = area.Height - childA.Height; //DODO investigate if smaller than minimum size
            }
        }

        void SetQuorridorPosition()
        {
            QuorridorPos = new Vector2(rnd.Next(0, area.Width), rnd.Next(0, area.Height));
        }

        void MakeRoom(ref Tile[,] grid)
        {
            roomBuilder = new RoomBuilder(grid, minSize, maxSize, TileType.MINER, new Vector2(area.X,area.Y));
            roomBuilder.GenerateRoom();
        }

        Location GetQuestLocation()
        {
            Location loc = new Location();
            loc.LocationType = TileType.MINER;
            return loc;
        }

        public void PathToSister(AstarPathing pathing)
        {
            if (children.Count > 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].PathToSister(pathing);
                }
            }
        }
    }
}
