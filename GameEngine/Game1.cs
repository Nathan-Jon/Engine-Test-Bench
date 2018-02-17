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

        Square ball1 = new Square();
        Square ball2 = new Square();
        Square ball3 = new Square();
        TestPlayer player = new TestPlayer();
        SATClass SAT;
        QuadTree quad;
        List<IAsset> objects;
       // List<IAsset> returnObjs;

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
            lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture.SetData(new Color[] { Color.White });
            objects = new List<IAsset>();
            quad = new QuadTree(0, new Rectangle(0, 0, ScreenWidth, ScreenHeight));

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
            ball1.setPos(400, 150);
            ball1.setTex(Content.Load<Texture2D>("square"));

            ball2.setPos(80, 90);
            ball2.setTex(Content.Load<Texture2D>("square"));

            ball3.setPos(20, 150);
            ball3.setTex(Content.Load<Texture2D>("square"));

            player.setPos(300, 300);
            player.setTex(Content.Load<Texture2D>("square"));




            objects.Add(player);
            objects.Add(ball1);
            objects.Add(ball2);
            objects.Add(ball3);

            SAT = new SATClass();

            for (int i = 0; i < objects.Count(); i++)
            {
                quad.InsertObj(objects[i]);
            }
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

            //Clear the list of Quads
            quad.Clear();


            //Rereate Quads lists and store objects
            for (int i = 0; i < objects.Count; i++)
            {
                quad.InsertObj(objects[i]);
            }



            List<IAsset> returnObjs = new List<IAsset>();
            for (int i = 0; i < objects.Count; i++)
            {
                returnObjs.Clear();
                quad.getList(returnObjs, objects[i]);

                for (int x = 0; x < returnObjs.Count(); x++)
                {
                    SAT.SquareVsSquare(objects[i], returnObjs[x++]) ;
                    Console.WriteLine("TESTING COLLISIONS");
                }
            }

            


            // TODO: Add your update logic here
            base.Update(gameTime);
            ball1.Update();
            ball2.Update();
            ball3.Update();
            player.Update();
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (SATClass._ColBool == true)
            {
                GraphicsDevice.Clear(Color.Blue);
            }
            else GraphicsDevice.Clear(Color.AntiqueWhite);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            ball1.Draw(spriteBatch);
            ball2.Draw(spriteBatch);
            ball3.Draw(spriteBatch);
            player.Draw(spriteBatch);

            IList<Vector2> ballPoints = ball1.getPoints();
            IList<Vector2> playerPoints = player.getPoints();
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
