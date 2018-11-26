using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class Grid
    {

    }
    class AstarPathing
    {
        // Grid grid;
        Tile[,] grid;
        public AstarPathing(/*Grid _grid*/ Tile[,] grid)
        {
            //   grid = _grid;
            this.grid = grid;
        }

        public List<Tile> FindPath(Tile _startNode, Tile _targetNode)
        {
            //Node startNode = grid.NodeFromWorldPoint(startPos);
            //Node targetNode = grid.NodeFromWorldPoint(targetPos);
            Tile startNode = _startNode;
            Tile targetNode = _targetNode;

            List<Tile> openSet = new List<Tile>();
            HashSet<Tile> closedSet = new HashSet<Tile>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Tile currentNode = openSet[0];               
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {                   
                    return RetracePath(startNode, targetNode);
                }

                foreach (Tile neighbour in GetNeighbours(currentNode))
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMoveCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMoveCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMoveCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            neighbour.Weight = 1;
                            openSet.Add(neighbour);
                        }

                    }
                }
            }
            Console.WriteLine("Found no path");
            return null;
        }

        List<Tile> RetracePath(Tile startNode, Tile targetNode)
        {
            List<Tile> path = new List<Tile>();
            Tile currenNode = targetNode;
            while (currenNode != startNode)
            {
                path.Add(currenNode);
                currenNode = currenNode.Parent;
               // currenNode.Color = Color.Blue;
            }
            path.Reverse();
            return path;
        }

        int GetDistance(Tile nodeA, Tile nodeB)
        {
            //   int distX = Math.Abs(nodeA.GridCoordX - nodeB.GridCoordX);
            //   int distY = Math.Abs(nodeA.GridCoordY - nodeB.GridCoordY);
            int distX = Math.Abs(nodeA.X - nodeB.X);
            int distY = Math.Abs(nodeA.Y - nodeB.Y);

            if (distX > distY)
                return 14 * distY + 10 * nodeB.Weight + (distX - distY);

            return 14 * distX + 10 * nodeB.Weight + (distY - distX);
        }

        public List<Tile> GetNeighbours(Tile node)
        {
            List<Tile> neighbours = new List<Tile>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++) // TODO Refactor
                {
                    if (x == 0 && y == 0)
                        continue;
                    if (x == 1 && y == -1)
                        continue;
                    if (x == -1 && y == -1)
                        continue;
                    if (x == 1 && y == 1)
                        continue;
                    if (x == 1 && y == -1)
                        continue;
                    //int checkX = node.GridCoordX + x;
                    //int checkY = node.GridCoordY + y;
                    int checkX = node.X + x;
                    int checkY = node.Y + y;

                    //if (checkX >= 0 && checkX < grid.gridSizeX && checkY >= 0 && checkY < grid.gridSizeY)
                    //    neighbours.Add(grid.nodes[checkY, checkX]);
                    if (checkX >= 0 && checkX < grid.GetUpperBound(1) && checkY >= 0 && checkY < grid.GetUpperBound(0))
                        neighbours.Add(grid[checkY, checkX]);
                }
            }
            return neighbours;
        }
    }
}

