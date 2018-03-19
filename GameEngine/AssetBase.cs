using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DemonstrationEngine

{
    class AssetBase: IAsset
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }

        public string Tag { get; set; }

        protected List<Vector2> Points = new List<Vector2>();
        protected List<Vector2> edges = new List<Vector2>();
        protected Vector2 _point1;
        protected Vector2 _point2;
        protected Vector2 _point3;
        protected Vector2 _point4;

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the object on screen
            SetPoints();
            spriteBatch.Draw(Texture, Points[0], Color.BlueViolet);
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

        public virtual void Update()
        {
        }

        public List<Vector2> Point()
        {
            return Points;
        }

        public List<Vector2> Edges()
        {
            return edges;
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


    }

}