using Microsoft.Xna.Framework;
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

        IAsset ball1;
        IAsset ball2;
        IAsset ball3;
        IAsset ball4;
        IAsset player;
        List<IAsset> Entities = new List<IAsset>(); 
        Vector2 Translation;
        NewSAT SAT;


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

            ball1 = new Square();
            ball2 = new Square();
            ball3 = new Square();
            ball4 = new Square();
            player = new TestPlayer();
            Entities.Add(ball1);
            Entities.Add(ball2);
            Entities.Add(ball3);
            Entities.Add(ball4);
            Entities.Add(player);
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
            ball1.setPos(new Vector2 (400, 100));
            ball1.setTex(Content.Load<Texture2D>("square"));

            ball2.setPos(new Vector2(100, 150));
            ball2.setTex(Content.Load<Texture2D>("square"));

            ball3.setPos(new Vector2(20, 60));
            ball3.setTex(Content.Load<Texture2D>("square"));

            ball4.setPos(new Vector2(200, 300));
            ball4.setTex(Content.Load<Texture2D>("square"));

            player.setPos(new Vector2(390, 140));
            player.setTex(Content.Load<Texture2D>("square"));
            


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
            ball1.Update();
            ball2.Update();
            ball3.Update();
            ball4.Update();
            player.Update();



            foreach (IAsset entity in Entities)
            {
                for (int i = 0; i < Entities.Count; i++)
                {
                    for (int x = 0; x < Entities.Count; x++)
                    {
                        if (x!= i)
                        {

                            Vector2 velocity = Entities[i].Velocity();
                            Translation = velocity;

                            SAT.PolygonVsPolygon(Entities[i], Entities[x], velocity);

                            if (SAT.WillIntersect)
                            {
                                Translation = velocity + SAT.MTV;
                                Entities[i].Position += SAT.MTV;
                                Entities[x].Position -= SAT.MTV;
                                Entities[i].Offset(Translation);
                            }

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

            GraphicsDevice.Clear(Color.AntiqueWhite);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            ball1.Draw(spriteBatch);
            ball2.Draw(spriteBatch);
            ball3.Draw(spriteBatch);
            ball4.Draw(spriteBatch);
            player.Draw(spriteBatch);
            IList<Vector2> ballPoints = ball1.Point();
            IList<Vector2> playerPoints = player.Point();

            //for (int i = 0; i < Entities.Count; i++)
            //{
            //    IList<Vector2> Points = Entities[i].Point();


            //    for (int x = 0; x < Points.Count; x++)
            //    {
            //        DrawLine(
            //        spriteBatch, Points[x], ballPoints[x + 1 == ballPoints.Count ? 0 : x + 1]
            //    );
            //        Points.Clear();

            //    }
            //}


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

        /// <summary>
        /// Called for the creation of lines between collidable entities
        /// </summary>
        /// <param name="sprtiBatch"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
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
