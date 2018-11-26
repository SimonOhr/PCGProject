using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PcgWorldGenOnStoryGen
{
    public enum TileType { GRASS, WOOD, CASTLE, DUNGEON, WATER, TOWN, NONE, MINER }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // QuestGen questGen;
        Texture2D spriteMap;
        BoardManager boardManager;
        public static int ScreenWidth, ScreenHeight;
        int tileSize;
        List<Map> maps;
        int selectedMap;
        KeyboardState oldKey, key;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteMap = Content.Load<Texture2D>("32x32_map_tile v3.1 [MARGINLESS]");
            InitGlobalVariables();
            SetScreenBounds();
            InitGlobalObjects();

        }

        void InitGlobalVariables()
        {
            tileSize = 32;
            ScreenWidth = tileSize * 50;
            ScreenHeight = tileSize * 30;
            maps = new List<Map>();
            selectedMap = 0;
        }

        void SetScreenBounds()
        {
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.ApplyChanges();
        }

        void InitGlobalObjects()
        {
            boardManager = new BoardManager(Window, spriteMap, ref maps);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            oldKey = key;
            key = Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1) && selectedMap + 1 < maps.Count && oldKey.IsKeyUp(Keys.NumPad1))
                selectedMap++;
            else if (Keyboard.GetState().IsKeyDown(Keys.NumPad1) && selectedMap + 1 > maps.Count)
                selectedMap = 0;
          
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            for (int i = 0; i < maps[selectedMap].tileMap.GetLength(0); i++)
            {
                for (int j = 0; j < maps[selectedMap].tileMap.GetLength(1); j++)
                {
                    maps[selectedMap].tileMap[i, j].Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
