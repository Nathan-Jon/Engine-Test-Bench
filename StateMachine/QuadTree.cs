using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class QuadTree
    {
        int maxObjects = 1;
        int maxLevels = 5;
        int level;
        List<IAsset> objects;
        Rectangle bounds;
        QuadTree[] nodes;

        public QuadTree(int Level, Rectangle Bounds)
        {
            this.level = Level;
            this.bounds = Bounds;
            objects = new List<IAsset>();
            nodes = new QuadTree[4];
        }

        public void Clear()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        public void Split()
        {
            int subNodeWidth = bounds.Width / 2;
            int subNodeHeight = bounds.Height / 2;
            int subNodeX = bounds.X;
            int subNodeY = bounds.Y;

            nodes[0] = new QuadTree(level + 1, new Rectangle(subNodeX + subNodeWidth, subNodeY, subNodeWidth, subNodeHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(subNodeX, subNodeY, subNodeWidth, subNodeHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(subNodeX, subNodeY + subNodeHeight, subNodeWidth, subNodeHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(subNodeX + subNodeWidth, subNodeY + subNodeHeight, subNodeWidth, subNodeHeight));
        }

        public void Insert(IAsset Entity)
        {
            if (nodes[0] != null)
            {
                int Index = getIndex(Entity);

                if (Index != -1)
                {
                    nodes[Index].Insert(Entity);
                    return;
                }
            }

            objects.Add(Entity);

            if (objects.Count() > maxObjects && level < maxLevels)
            {
                if (nodes[0] == null)
                {
                    Split();
                }
               
                int i = 0;
                while (i < objects.Count())
                {
                    int Index = getIndex(objects[i]);
                    if (Index != -1)
                    {
                        nodes[Index].Insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

        }

        public int getIndex(IAsset Entity)
        {
            int Index = -1;
            float YMidpoint = bounds.X + (bounds.Width / 2);
            float XMidpoint = bounds.Y + (bounds.Height / 2);

            bool topQuad = (Entity.Position.Y < XMidpoint && Entity.Position.Y + Entity.Texture.Height < XMidpoint);
            bool bottomQuad = (Entity.Position.Y > XMidpoint);

            if (Entity.Position.X < YMidpoint && Entity.Position.X + Entity.Texture.Width < YMidpoint)
            {
                if (topQuad)
                {
                    Index = 1;
                }
                else if (bottomQuad)
                {
                    Index = 2;
                }
            }
            else if (Entity.Position.X > YMidpoint)
            {
                if (topQuad)
                {
                    Index = 0;
                }
                else if (bottomQuad)
                {
                    Index = 3;
                }
            }

            return Index;
        }

        public List<IAsset> retrieve(List<IAsset> returnObjects, IAsset Entity)
        {
            int Index = getIndex(Entity);
            if (Index != -1 && nodes[0] != null)
            {
                nodes[Index].retrieve(returnObjects, Entity);
            } else if (nodes[0] != null)
            {
                foreach (QuadTree node in nodes)
                {
                    node.retrieve(returnObjects, Entity);
                }
            }

            foreach (IAsset item in objects)
            {
                if (item != Entity)
                    returnObjects.Add(item);
            }

            return returnObjects;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tex)
        {
            //Draws the object on screen

            spriteBatch.Draw(tex, new Rectangle(bounds.X, bounds.Y, bounds.Width, 3), Color.White);
            spriteBatch.Draw(tex, new Rectangle(bounds.X, bounds.Y + bounds.Height, 3, bounds.Height), Color.White);
            spriteBatch.Draw(tex, new Rectangle(bounds.X + bounds.Width, bounds.Y, 3, bounds.Height), Color.White);
            spriteBatch.Draw(tex, new Rectangle(bounds.X, bounds.Y, 3, bounds.Height), Color.White);

            if (nodes[0] != null)
            {
                foreach (QuadTree node in nodes)
                {
                    node.Draw(spriteBatch, tex);
                }
            }

        }
    }
}
