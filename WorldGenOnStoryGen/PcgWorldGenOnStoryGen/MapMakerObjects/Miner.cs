using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class Miner
    {
        Direction directionManager;
        Tile[] nextArrayOfTiles;
        Tile[,] grid;
        Random rnd;
        Tile pos, savedPos;
        List<Tile> markedTrail;
        List<Tile> tempMarkedTrail;
        int tileSize;
        int stepped = 0;
        Vector2 newDirection;

        public int StepsInSteps { get; set; }
        public int Steps { get; set; }

        public Miner(MapMaker mk, Tile startTile)
        {
            grid = mk.Tiles;
            pos = startTile;
            InitiateGlobalVariables();
        }

        void InitiateGlobalVariables()
        {
            Steps = 4;
            rnd = new Random();
            tileSize = grid[0, 0].TileSize;

            tempMarkedTrail = new List<Tile>();
            directionManager = new Direction(grid);
            tempMarkedTrail.Add(pos);
        }

        public void StartMiner(MapMaker mk)
        {
            markedTrail = mk.Trail;
            //    StartTile();
            int i = 0;
            while (i < Steps)
            {
                Step();
                i++;
            }
        }

        //void StartTile()
        //{
        //    //int indexX = new Random().Next(grid.GetLength(1)); //TODO rnd BUG using Random() for now
        //    //int indexY = new Random().Next(grid.GetLength(0));

        //}

        void Step()
        {
            StepsInSteps = rnd.Next(3, 6);
            int noOverflow = 0;
            nextArrayOfTiles = null;
            savedPos = pos;
            Console.WriteLine("startPos on Step, X={0},Y={1}", pos.X, pos.Y);
            while (nextArrayOfTiles == null)
            {
                nextArrayOfTiles = StepIteration(StepsInSteps);

                noOverflow++;
                if (noOverflow > 50)
                {
                    break;
                }
            }
            try
            {
                foreach (Tile tile in nextArrayOfTiles)
                {
                    tile.SetTileType(TileType.MINER);
                }

                Console.WriteLine($"Finished Next Pos iteration...");
                for (int i = 0; i < nextArrayOfTiles.Length; i++)
                {
                    Console.WriteLine($"nextPos {i} = [{nextArrayOfTiles[i].Y},{nextArrayOfTiles[i].X}] is end of path = {nextArrayOfTiles[i].IsEndOfPath}");
                }
                pos = nextArrayOfTiles.Last();
               // pos.IsEndOfPath = true;
                pos.SetTileDirection(newDirection);

                tempMarkedTrail.AddRange(nextArrayOfTiles);

                if (++stepped == Steps)
                {
                    SuccessfulMiner();
                }
            }
            catch (Exception ex)
            {
                FailedMiner();
            }
        }

        void SuccessfulMiner()
        {
            tempMarkedTrail.Last().IsEndOfPath = true;
            markedTrail.AddRange(tempMarkedTrail);
        }

        void FailedMiner()
        {
            tempMarkedTrail.Clear();
        }

        Tile[] StepIteration(int numberOfSteps)
        {
            Tile[] nextSetOfTiles = new Tile[numberOfSteps];

            Console.WriteLine($"pos on StepIteration = [{pos.X},{pos.Y}]");


            Vector2 vectPos = new Vector2((pos.Dest.X / tileSize), (pos.Dest.Y / tileSize));
            newDirection = GetNewDirection();

            vectPos += (newDirection * numberOfSteps);

            if (vectPos.X > grid.GetUpperBound(1) || vectPos.X < grid.GetLowerBound(1))
                nextSetOfTiles = null;
            else if (vectPos.Y > grid.GetUpperBound(0) || vectPos.Y < grid.GetLowerBound(0))
                nextSetOfTiles = null;
            else
            {
                int tempY = pos.Y;
                int tempX = pos.X;
                for (int i = 0; i < numberOfSteps; i++)
                {
                    tempY += (int)newDirection.Y;
                    tempX += (int)newDirection.X;
                    if (!grid[tempY, tempX].IsWalkable)
                    {
                        nextArrayOfTiles = null;
                        break;
                    }
                    nextSetOfTiles[i] = grid[tempY, tempX];
                }
            }

            if (NotNull(nextSetOfTiles) && NoCollition(nextSetOfTiles))
            {
                Console.WriteLine($"Both Ensures returned true, should be good to go");
                Console.WriteLine($"pos on StepIterated = [{pos.Y},{pos.X}]");
                Console.WriteLine($"direction on stepIterated = {newDirection}");
                return nextSetOfTiles;
            }
            else
            {
                return null;
            }
        }

        Vector2 GetNewDirection()
        {
            return directionManager.GetDirection(pos);
        }

        bool NoCollition(Tile[] nextSetOfTiles)
        {
            Console.WriteLine("Ensuring No OverLap...");

            int counter = 0;

            for (int i = 0; i < tempMarkedTrail.Count; i++)
            {
                for (int j = 0; j < nextSetOfTiles.Length; j++)
                {
                    if (tempMarkedTrail[i].hitbox.Contains(nextSetOfTiles[j].hitbox))
                        counter++;

                    else if (markedTrail[i].hitbox.Contains(nextSetOfTiles[j].hitbox))
                        counter++;
                }
            }
            if (counter > 0)
            {
                Console.WriteLine("false");
                return false;
            }
            Console.WriteLine("true");
            return true;
        }

        bool NotNull(Tile[] nextSetOfTiles)
        {
            Console.WriteLine($"Ensuring Values Not Null...");
            int counter = 0;
            if (nextSetOfTiles != null)
                for (int i = 0; i < nextSetOfTiles.Length; i++)
                {
                    if (nextSetOfTiles[i] != null)
                    {
                        counter++;
                    }
                }

            if (nextSetOfTiles != null && counter == nextSetOfTiles.Length)
            {
                Console.WriteLine($"true counter {counter} = {nextSetOfTiles.Length}");
                return true;
            }

            Console.WriteLine($"false (counter = {counter})");
            return false;
        }

        bool BoundsCheck(int nextX, int nextY)
        {
            if (nextY > grid.GetLowerBound(0) && nextY < grid.GetUpperBound(0))
                if (nextX > grid.GetLowerBound(1) && nextX < grid.GetUpperBound(1))
                {
                    return true;
                }
            return false;
        }

        public List<Tile> GetTrail()
        {
            return markedTrail;
        }
    }
}
