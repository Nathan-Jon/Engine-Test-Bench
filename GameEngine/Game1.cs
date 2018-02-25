﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GameEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int ScreenHeight, ScreenWidth;

        private Texture2D lineTexture;

        IAsset Square;
        IAsset player;
        //IAsset ball1;
        //IAsset ball2;
        NewSAT SAT;
        QuadTree quad;
        bool coli = false;
        List<IAsset> Entities = new List<IAsset>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 900;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ScreenHeight = GraphicsDevice.Viewport.Height;
            ScreenWidth = GraphicsDevice.Viewport.Width;
            lineTexture = new Texture2D(this.GraphicsDevice, 1, 1);
            lineTexture.SetData<Color>(new Color[] { Color.White });

            SAT = new NewSAT();

            Square = new Square();
            player = new TestPlayer();
            //ball1 = new Square();
            //ball2 = new Square();
            Entities.Add(player);
            Entities.Add(Square);
            //Entities.Add(ball1);
            //Entities.Add(ball2);
            quad = new QuadTree(0, new Rectangle(0, 0, ScreenWidth, ScreenHeight));

            this.IsMouseVisible = true;

            base.Initialize();

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Square.setPos(new Vector2 (100, 100));
            Square.setTex(Content.Load<Texture2D>("square"));

            player.setPos(new Vector2(200, 200));
            player.setTex(Content.Load<Texture2D>("square"));

            //ball1.setPos(new Vector2(300, 300));
            //ball1.setTex(Content.Load<Texture2D>("square"));

            //ball2.setPos(new Vector2(400, 400));
            //ball2.setTex(Content.Load<Texture2D>("square"));



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            // TODO: Add your update logic here
            base.Update(gameTime);

            quad.Clear();
            for (int i = 0; i < Entities.Count; i++)
            {
                quad.Insert(Entities[i]);
            }

            List<IAsset> returnObjects = new List<IAsset>();
            for(int i = 0; i < Entities.Count; i++)
            {
                returnObjects.Clear();
                returnObjects = quad.retrieve(returnObjects, Entities[i]);

                for (int x = 0; x < returnObjects.Count; x++)
                {
                    coli = true;
                }

            }


            for (int e = 0; e < Entities.Count; e++)
                {
                Entities[e].Update();
                }

                for (int i = 0; i < Entities.Count; i++)
                {
                    for (int x = 0; i < Entities.Count; i++)
                    {

                        if (x != i)
                        {
                            SAT.PolygonVsPolygon(Entities[i], Entities[x]);

                            if (SAT.Intersect)
                            {
                                Entities[i].Position += SAT.MTV;
                                Entities[x].Position -= SAT.MTV;
                            }
                        }
                    }
                }
            }
            

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (coli == true)
            {
                GraphicsDevice.Clear(Color.Red);
                coli = false;
            }
            else
            {
                GraphicsDevice.Clear(Color.AntiqueWhite);
            }
            

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            Square.Draw(spriteBatch);
            player.Draw(spriteBatch);
            //ball1.Draw(spriteBatch);
            //ball2.Draw(spriteBatch);
            IList<Vector2> ballPoints = Square.Point();
            IList<Vector2> playerPoints = player.Point();
            for (int i = 0; i < ballPoints.Count; i++)
            {
                DrawLine(
                    spriteBatch, ballPoints[i], ballPoints[i + 1 == ballPoints.Count ? 0 : i + 1]
                );
            }

            for (int i = 0; i < playerPoints.Count; i++)
            {
                DrawLine(
                    spriteBatch, playerPoints[i], playerPoints[i + 1 == playerPoints.Count ? 0 : i + 1]
                );
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void DrawLine(SpriteBatch sprtiBatch, Vector2 start, Vector2 end)
        {
            // some fancy shit to be able to draw lines between 2 points
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(
                lineTexture,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 3),
                null,
                Color.Red,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0
            );
        }
    }
}
