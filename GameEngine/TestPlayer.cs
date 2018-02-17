using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    class TestPlayer : IAsset 
    {
        public Texture2D Object;
        public Vector2 Locn;
        public float force = 4;


        //Create variables for the points
        //List to Store pont Variables
        public List<Vector2> Points = new List<Vector2>();
        Vector2 _point1;
        Vector2 _point2;
        Vector2 _point3;
        Vector2 _point4;
       

        public void setPos(float Xpos, float Ypos)
        {
            Locn.X = Xpos;
            Locn.Y = Ypos;
        }
        public void setTex(Texture2D tex)
        {
            Object = tex;
        }
        public Vector2 getPos
        {
            get { return Locn; }
        }
        public Texture2D getTex
        {
            get { return Object; }
        }
        public void KeyBoardMove()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D))
            { Locn.X += 4;}
            if (state.IsKeyDown(Keys.A))
            { Locn.X -= 4; }
            if (state.IsKeyDown(Keys.S))
            { Locn.Y += 4; }
            if (state.IsKeyDown(Keys.W))
            { Locn.Y -= 4; }
        }

        public void setPoints()
        {
            //Top Left
            _point1 = new Vector2(getPos.X, getPos.Y);
            //Top Right
            _point2 = new Vector2((getPos.X + getTex.Width), getPos.Y);
            //Bottom Right
            _point3 = new Vector2((getPos.X + getTex.Width), (getPos.Y + getTex.Height));
            //Bottom Left
            _point4 = new Vector2(getPos.X, (getPos.Y + getTex.Height));


            Points.Add(_point1);
            Points.Add(_point2);
            Points.Add(_point3);
            Points.Add(_point4);
        }
        public List<Vector2> getPoints()
        {
            Points.Clear();
            setPoints();
            return Points;
        }

        //Create the Edges and returns the Axies from each edge
        public List<Vector2> getAxies()
        {
            List<Vector2> Axies = new List<Vector2>();
            //Get the edges between each point
            for (int i = 0; i < Points.Count; i++)
            {
                //Edges are created by subtracting points between two points(vertices)
                Vector2 _edge = Points[i] - Points[i + 1 == Points.Count ? 0 : i + 1];
                //Find the Normal of the edge
                _edge.Normalize();
                Axies.Add(_edge);
            }
            return Axies;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the object on screen
            spriteBatch.Draw(getTex, getPos, Color.BlueViolet);
            //test();
        }
        
        public void CollisionDetection()
        {
            //Side Of Screen//

            if (getPos.X >= 850)
            {
               Locn.X = 850;

            }
            if (getPos.X <= 0)
            {
                Locn.X = 0;
            }
            if (getPos.Y >= 550)
            {
                Locn.Y = 550;
            }
            if (getPos.Y <= 0)
            {
                Locn.Y = 0;
            }

        }

        public void Update()
        {
            KeyBoardMove();
            
            CollisionDetection();
        }
    }
}
