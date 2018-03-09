using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StateMachine.StateMachine;

namespace GameEngine
{
    class TestPlayer : AssetBase, IAsset
    {

        public float ForceX = 5;
        public float ForceY = 5;

        //Create variables for the points
        //List to Store pont Variables
        private List<Vector2> Points = new List<Vector2>();
        public List<Vector2> edges = new List<Vector2>();
        Vector2 _point1;
        Vector2 _point2;
        Vector2 _point3;
        Vector2 _point4;

        public TestPlayer()
        {
           StateMachine<IAsset> stateMachine = new StateMachine<IAsset>(this);
        
            stateMachine.AddMethodTransition(stateChange, "state1", "state2");
            stateMachine.AddMethodTransition(stateChange2, "state2", "state1");
           

        }



        //State Methods
        bool stateChange()
        {
            if (Position.X > 300)
                return true;
            return false;
        }
        bool stateChange2()
        {
            if (Position.X < 300)
                return true;
            return false;
        }
        
















        public void KeyBoardMove()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                ApplyForce(new Vector2(-ForceX,0));
            }
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                ApplyForce(new Vector2(ForceX, 0));
            }
            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
            {
                ApplyForce(new Vector2(0, -ForceY));
            }
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
            {
                ApplyForce(new Vector2(0,ForceY));
            }
        }

        public void SetPoints()
        {
            Points.Clear();
            //Top Left
            _point1 = new Vector2(Position.X, Position.Y);
            //Top Right
            _point2 = new Vector2((Position.X + Texture.Width), Position.Y);
            //Bottom Right
            _point3 = new Vector2((Position.X + Texture.Width), (Position.Y + Texture.Height));
            //Bottom Left
            _point4 = new Vector2(Position.X, (Position.Y + Texture.Height));


            Points.Add(_point1);
            Points.Add(_point2);
            Points.Add(_point3);
            Points.Add(_point4);

            BuildEdges();
        }

        public void BuildEdges()
        {
            Vector2 p1;
            Vector2 p2;
            edges.Clear();
            for (int i = 0; i < Points.Count; i++)
            {
                p1 = Points[i];
                if (i + 1 >= Points.Count)
                {
                    p2 = Points[0];
                }
                else
                {
                    p2 = Points[i + 1];
                }
                edges.Add(p2 - p1);
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the object on screen
            spriteBatch.Draw(Texture, Points[0], Color.BlueViolet);
        }
        
        public void CollisionDetection()
        {
            //Side Of Screen//

            if (Position.X >= 850)
            {
                Position = (new Vector2(850, Position.Y));
            }
            if (Position.X <= 0)
            {
                Position = (new Vector2(0, Position.Y));
            }
            if (Position.Y >= 550)
            {
                Position = (new Vector2(Position.X, 550));
            }
            if (Position.Y <= 0)
            {
                Position = (new Vector2(Position.X, 0));
            }

        }

        public void Update()
        {
            SetPoints();
            KeyBoardMove();          
            CollisionDetection();
            UpdatePhysics();
        }

        public float Radius()
        {
            float radius = Texture.Width / 2;
            return radius;
        }

        public Vector2 Center()
        {
                float totalX = 0;
                float totalY = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    totalX += Points[i].X;
                    totalY += Points[i].Y;
                }
                return new Vector2(totalX / Points.Count, totalY / Points.Count);
        }

        public void Offset(Vector2 translation)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Vector2 p = Points[i];
                Points[i] = new Vector2(p.X + translation.X, p.Y + translation.Y);
            }

        }


        public List<Vector2> Edges()
        {
            return edges;
        }

        public List<Vector2> Point()
        {
            return Points;
        }
        public Vector2 Velocity()
        {
            return new Vector2(0,0);
        }

        public void setPos(Vector2 locn)
        {
            Position = locn;
        }
        public void setTex(Texture2D tex)
        {
            Texture = tex;
        }
    }
}
