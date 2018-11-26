using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    class Direction
    {
        Random rnd;
        Vector2 direction, center, position;
        Tile[,] grid;
        Rectangle upperLeft, lowerLeft, UpperRight, lowerRight;
        int boundsXUpper, boundsYUpper, boundsXLower, boundsYLower, tileSize;

        public Direction(Tile[,] grid)
        {
            this.grid = grid;
            rnd = new Random();
            direction = Vector2.Zero;
            GetGridBounds();
        }

        void GetGridBounds()
        {
            boundsYLower = 0;
            boundsXLower = 0;

            boundsYUpper = grid.GetUpperBound(0);
            boundsXUpper = grid.GetUpperBound(1);

            tileSize = grid[0, 0].TileSize;

            center = new Vector2((boundsXUpper / 2), (boundsYUpper / 2));
        }

        Vector2 CalculateDirection()
        {
            Vector2 targetPos;
            int chance = new Random().Next(0, 3);
            if (position.X < center.X)
            {
                if (position.Y < center.Y)
                {


                    if (chance == 0)
                    {
                        targetPos.X = rnd.Next(boundsXLower, (int)center.X);//lowerLeft
                        targetPos.Y = rnd.Next((int)center.Y, (int)center.Y);//lowerLeft
                    }
                    else if (chance == 1)
                    {
                        targetPos.X = rnd.Next((int)center.X, boundsXUpper);//uppeRight
                        targetPos.Y = rnd.Next(boundsYLower, (int)center.Y);//upperRight
                    }
                    else
                    {
                        targetPos.X = rnd.Next((int)center.X, boundsXUpper);//lowereRight
                        targetPos.Y = rnd.Next((int)center.Y, (int)center.Y);//lowerright
                    }
                }
                else
                {
                    if (chance == 0)
                    {
                        targetPos.X = rnd.Next(boundsXLower, (int)center.X);// upperLEft
                        targetPos.Y = rnd.Next(boundsYLower, (int)center.Y);//UpperLeft
                    }
                    else if (chance == 1)
                    {
                        targetPos.X = rnd.Next((int)center.X, boundsXUpper);//uppeRight
                        targetPos.Y = rnd.Next(boundsYLower, (int)center.Y);//upperRight
                    }
                    else
                    {
                        targetPos.X = rnd.Next((int)center.X, boundsXUpper);//lowereRight
                        targetPos.Y = rnd.Next((int)center.Y, (int)center.Y);//lowerright
                    }
                }
            }
            else
            {
                if (position.Y < center.Y)
                {

                    if (chance == 0)
                    {
                        targetPos.X = rnd.Next(boundsXLower, (int)center.X);// upperLEft
                        targetPos.Y = rnd.Next(boundsYLower, (int)center.Y);//UpperLeft
                    }
                    else if (chance == 1)
                    {
                        targetPos.X = rnd.Next(boundsXLower, (int)center.X);//lowerLeft
                        targetPos.Y = rnd.Next((int)center.Y, (int)center.Y);//lowerLeft
                    }
                    else
                    {
                        targetPos.X = rnd.Next((int)center.X, boundsXUpper);//lowereRight
                        targetPos.Y = rnd.Next((int)center.Y, (int)center.Y);//lowerright
                    }
                }
                else
                {
                    if (chance == 0)
                    {
                        targetPos.X = rnd.Next(boundsXLower, (int)center.X);// upperLEft
                        targetPos.Y = rnd.Next(boundsYLower, (int)center.Y);//UpperLeft
                    }
                    else if (chance == 1)
                    {
                        targetPos.X = rnd.Next(boundsXLower, (int)center.X);//lowerLeft
                        targetPos.Y = rnd.Next((int)center.Y, (int)center.Y);//lowerLeft
                    }
                    else
                    {
                        targetPos.X = rnd.Next((int)center.X, boundsXUpper);//uppeRight
                        targetPos.Y = rnd.Next(boundsYLower, (int)center.Y);//upperRight
                    }
                }
            }

            targetPos = new Vector2(targetPos.X - position.X, targetPos.Y - position.Y);

            targetPos.Normalize();

            return targetPos;
        }

        public Vector2 GetDirection(Tile start)
        {
            direction = Vector2.Zero;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == start)
                    {
                        position = new Vector2(j, i);
                    }
                }
            }
            // Vector2 distancefromCenter = center - position; // not used, might have use for it in the future           
            while (direction == Vector2.Zero)
            {
                direction = CalculateDirection();
                float x = Math.Abs(direction.X);
                float y = Math.Abs(direction.Y);
                if (x > y)
                {
                    if (direction.X > 0)
                        direction.X = 1;
                    else
                        direction.X = -1;
                    direction.Y = 0;
                }
                else
                {
                    if (direction.Y > 0)
                        direction.Y = 1;
                    else
                        direction.Y = -1;
                    direction.X = 0;
                }
            }
            return direction;
        }
    }
}
