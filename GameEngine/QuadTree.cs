using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    class QuadTree
    {
        //Max number of Objects and levels to be held by a quadtree
        private int MaxObjs;
        private int MaxLevels;

        //Node Level
        private int NodeLevel;

        private List<IAsset> Objects;
        private Rectangle Bounds;
        private QuadTree[] Nodes;


        public QuadTree(int level, Rectangle area)
        {
            NodeLevel = level;
            Objects = new List<IAsset>();
            Bounds = area;
            Nodes = new QuadTree[4];

        }


        /// <summary>
        /// Clears the QuadTrees - Removing each node of their collidable entities
        /// </summary>
        public void Clear()
        {
            //clear Objects List
            Objects.Clear();
            //Clear the Lists for each of the Nodes
            for (int i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i] != null)
                {
                    Nodes[i].Clear();
                    Nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Divides the Nodes into 4 subnodes
        /// </summary>
        public void Split()
        {
            int subWidth = (Bounds.Width / 2);
            int subHeight = (Bounds.Height / 2);
            int x = Bounds.X;
            int y = Bounds.Y;
            
            QuadTree Node1 = new QuadTree(NodeLevel + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));             //Place node in top right
            QuadTree Node2 = new QuadTree(NodeLevel + 1, new Rectangle(x, y, subWidth, subHeight));                        //Place Node in top Left
            QuadTree Node3 = new QuadTree(NodeLevel + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));            //Place Node in Bottom Left
            QuadTree Node4 = new QuadTree(NodeLevel + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight)); //Place Node in Bottom Right

            //Add Each Node to the Nodes Array
            Nodes[0] = Node1;
            Nodes[1] = Node2;
            Nodes[2] = Node3;
            Nodes[3] = Node4;
        }

        /// <summary>
        /// Determines Which node and entity belongs to
        /// -1 means the object doesnt fit and is calculated in the parent node
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        private int GetIndex(IAsset ent)
        {
            int index = -1;

            //The central points of the bounding area
            double HorizontalPoint = Bounds.X + Bounds.Width / 2;
            double VerticalPoint = Bounds.Y + Bounds.Height / 2;

            //Object Fits into Top quadrant
            bool TopQuad = (ent.Position.Y < HorizontalPoint && ent.Position.X + ent.GetTex().Width < VerticalPoint);
            //Object fits into Lower Quadrants
            bool LowQuad = (ent.Position.Y > HorizontalPoint);


            //Test if entities fit in Left quadrants
            if (ent.Position.X < VerticalPoint && ent.Position.X + ent.GetTex().Width < VerticalPoint)
            {
                if (TopQuad)
                    index = 1;
                else
                    index = 2;
            }
            //Test if entities fit in Right quadrants
            else if (LowQuad)
            {
                if (TopQuad)
                {
                    index = 0;
                }
                else if (LowQuad)
                {
                    index = 3;
                }
            }
            return index;
        }

        /// <summary>
        /// PLace Objects into the quad trees
        /// if the list exceeds the maxvalue, split()
        /// </summary>
        /// <param name="ent"></param>
        public void InsertObject(IAsset ent)
        {
            //If there are nodes
            if (Nodes[0] != null)
            {
                //Identify what node the entity is currently in
                int index = GetIndex(ent);

                if (index != -1) 
                {
                    //Store entity in a nodes list
                    Nodes[index].InsertObject(ent);
                }
                return;
            }

            //Store entities in this Node's Objects list
            Objects.Add(ent);

            if (Objects.Count() > MaxObjs && NodeLevel < MaxLevels)
            {
                //If there are no nodes, call the split method to create them.
                if (Nodes[0] == null)
                    Split();

                int i = 0;

                //While the count is greater than i, place objects into the nodes list
                while (i < Objects.Count())
                {
                    int index = GetIndex(Objects[i]);

                    if (index != -1)
                    {
                        Nodes[index].InsertObject(Objects[i]);
                    }
                    else i++;
                }
            }
        }


        //Get Objects position in their boxes

        //Place Objects into Quad Tree

        //Catch the objects within a node

    }
}
