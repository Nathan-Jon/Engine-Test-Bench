using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    class Square : AssetBase, IAsset
    {
        public Texture2D Object;

        public float ForceX = 1;
        public float ForceY = 2;

        //Create variables for the points
        //List to Store pont Variables
        private List<Vector2> Points = new List<Vector2>();
        private List<Vector2> edges = new List<Vector2>();
        Vector2 _point1;
        Vector2 _point2;
        Vector2 _point3;
        Vector2 _point4;



        //STORE THE POINTS IN THE VARIABLES
        public void setPoints()
        {
            Points.Clear();
            //Top Left
            _point1 = new Vector2(Position.X, Position.Y);
            //Top Right
            _point2 = new Vector2((Position.X + GetTex().Width), Position.Y);
            //Bottom Right
            _point3 = new Vector2((Position.X + GetTex().Width), (Position.Y + GetTex().Height));
            //Bottom Left
            _point4 = new Vector2(Position.X, (Position.Y + GetTex().Height));


            Points.Add(_point1);
            Points.Add(_point2);
            Points.Add(_point3);
            Points.Add(_point4);

            BuildEdges();
        }
     
        public void setPos(Vector2 Posn)
        {
            Position = Posn;
        }
        public void setTex(Texture2D tex)
        {
            Object = tex;
        }
        //public Vector2 getPos
        //{
        //    get { return Position; }
        //}
        public Texture2D GetTex()
        {
            return Object;
        }
        public void move()
        {
            ApplyForce(new Vector2(-ForceX, 0));
            // Locn += velocity * facing;
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
            spriteBatch.Draw(GetTex(), Points[0], Color.AntiqueWhite);
        }

        public void CollisionDetection()
        {
            //Side Of Screen//

            if (Position.X >= 850)
            {
                Position = (new Vector2(850, Position.Y));
                ForceX = ForceX * -1;

            }
            if (Position.X <= 0)
            {
                Position = (new Vector2(0, Position.Y));
                ForceX = ForceX * -1;
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
           // move();
            setPoints();
            CollisionDetection();
            UpdatePhysics();
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

                return new Vector2(totalX / (float)Points.Count, totalY / (float)Points.Count);
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
            return new Vector2(0, 0);
        }
    }
}
