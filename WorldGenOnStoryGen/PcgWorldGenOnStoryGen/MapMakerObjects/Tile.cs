using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PcgWorldGenOnStoryGen
{
    class Tile
    {
        TileType selectedType;
        Texture2D tex;
        Random rnd;
        Vector2 srcVect;
        Rectangle src;
        //Color Color;
        Vector2 direction;

        public Rectangle hitbox;

        public Vector2 DestVect { get; set; }
        public Rectangle Dest { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public bool IsEndOfPath { get; set; } //flag
        public int TileSize { get; private set; }
        public bool ContainsEnemy { get; set; }
        public bool IsEntry { get; set; }
        public bool IsBranchTile { get; set; }
        //pathing variables
        public int Weight { get; set; }
        public bool IsWalkable { get; set; }
        public int HCost { get; set; }
        public int GCost { get; set; }
        public int FCost
        {
            get
            {
                return HCost + GCost;
            }
        }
        public Tile Parent { get; set; }
        public Color Color { get; set; }


        public Tile(Texture2D tex, Vector2 dest, int indexX, int indexY, int weight, TileType defaultType = TileType.NONE)
        {
            this.tex = tex;
            DestVect = dest;
            selectedType = defaultType;
            X = indexX;
            Y = indexY;
            Weight = weight;
            InitValues();
            hitbox = new Rectangle((X * TileSize), (Y * TileSize), TileSize, TileSize);
        }

        void InitValues()
        {
            rnd = new Random();
            TileSize = 32;
            //Weight = SetWeight();          
            
            src = new Rectangle((int)GetMapValues().X, (int)GetMapValues().Y, TileSize, TileSize);
            Dest = new Rectangle((int)DestVect.X, (int)DestVect.Y, TileSize, TileSize);
            Color = new Color();
            CheckIfEnemyOnTile();
            IsWalkable = true;
        }

        public void SetWeight(int _weight)
        {
            Weight = _weight;
        }

        public void RollBranchTile(int rnd)
        {
            int result = rnd;
            if (result <= 1)
                IsBranchTile = true;
            else
            IsBranchTile = false;
        }

        public void UpdateRectangle(int indexX, int indexY)
        {
            hitbox.X = (indexX * TileSize);
            hitbox.Y = (indexY * TileSize);
        }

        public void SetTileType(TileType type)
        {
            selectedType = type;
            srcVect.X = GetMapValues().X;
            srcVect.Y = GetMapValues().Y;
            src = new Rectangle((int)srcVect.X, (int)srcVect.Y, TileSize, TileSize);
            //src.X = (int)GetMapValues().X;
            //src.Y = (int)GetMapValues().Y;


        }

        Vector2 GetMapValues()
        {
            if (selectedType == TileType.GRASS)
            {
                return TextureCoordinates.Grass;
            }

            else if (selectedType == TileType.WOOD)
            {
                return TextureCoordinates.Wood;
            }

            else if (selectedType == TileType.WATER)
            {
                return TextureCoordinates.Grass; //TODO: Decide on water
            }

            else if (selectedType == TileType.CASTLE)
            {
                return TextureCoordinates.Castle;
            }

            else if (selectedType == TileType.DUNGEON)
            {
                return TextureCoordinates.Dungeon;
            }

            else if (selectedType == TileType.TOWN)
            {
                return TextureCoordinates.Town;
            }

            else if (selectedType == TileType.MINER)
            {
                return TextureCoordinates.Miner;
            }
            return Vector2.Zero;
        }

        public TileType GetTileType()
        {
            return selectedType;
        }

        void CheckIfEnemyOnTile()
        {
            if (ContainsEnemy || IsEntry)
                Color = Color.Red;
            else
                Color = Color.White;
        }

        public void Draw(SpriteBatch sb)
        {
            if (IsEntry)
            {
                //Color = Color.Red;
                SetTileType(TileType.MINER); //TODO refactor look at astar include last/first element in path
            }
            if (src != null)
                sb.Draw(tex, Dest, src, Color);
        }

        public Vector2 GetTileDirection()
        {
            return direction;
        }

        public void SetTileDirection(Vector2 tileDirection)
        {
            direction = tileDirection;
        }

    }
}
