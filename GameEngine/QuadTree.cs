using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DemonstrationEngine

{
    public class QuadTree
    {
        int maxObjects = 2;
        int maxLevels = 4;
        int level;
        List<IAsset> objects;
        Rectangle bounds;
        QuadTree[] nodes;
        float XMidpoint;
        float YMidpoint;

        public QuadTree(int Level, Rectangle Bounds)
        {
            level = Level;
            bounds = Bounds;
            objects = new List<IAsset>();
            nodes = new QuadTree[4];
            Game1.quadList.Add(this);

            XMidpoint = bounds.X + (bounds.Width / 2);    //Determines he center point of the bounds X value
            YMidpoint = bounds.Y + (bounds.Height / 2);   //Determines he center point of the bounds y value
        }

        /// <summary>
        /// Clear the Objects Lists from each and every Node
        /// </summary>
        public void Clear()
        {
            objects.Clear(); //Clear all of the Entities from the Objects List

            Game1.quadList.Remove(this); //Remove this Object from the drawList in Game1

            for (int i = 0; i < nodes.Length; i++) //For Every Node
            {
                if (nodes[i] != null) // IF there is a Node
                {
                    nodes[i].Clear(); //Clear the Objects lists
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Creates Sub Nodes when Called
        /// </summary>
        public void Split()
        {

            int subNodeWidth = bounds.Width / 2; //The width of the subnode
            int subNodeHeight = bounds.Height / 2; //Height of the subnode
            int subNodeX = bounds.X; //X Pos of the Parent Node
            int subNodeY = bounds.Y; //Y pos of the Parent Node


            //Create New Node in the top Left
            nodes[0] = new QuadTree(level + 1, new Rectangle(subNodeX, subNodeY, subNodeWidth, subNodeHeight));
            //Create New Node in Bottom Left
            nodes[1] = new QuadTree(level + 1, new Rectangle(subNodeX, subNodeY + subNodeHeight, subNodeWidth, subNodeHeight));
            //Create New Node int the Top Right
            nodes[2] = new QuadTree(level + 1, new Rectangle(subNodeX + subNodeWidth, subNodeY, subNodeWidth, subNodeHeight));
            //Create New Node in Bottom Right
            nodes[3] = new QuadTree(level + 1, new Rectangle(subNodeX + subNodeWidth, subNodeY + subNodeHeight, subNodeWidth, subNodeHeight));
        }

        /// <summary>
        /// Stores the Entity into the Nodes
        /// </summary>
        /// <param name="Entity"></param>
        public void Insert(IAsset Entity)
        {
            //======================================= If there ARE nodes ==============================================\\

            #region Existing Nodes Details
            if (nodes[0] != null) //If there are Sub Nodes
            {
                int Index = GetIndex(Entity); // Get the Index Value

                if (Index != -1) //If the value isn't -1 
                {
                    nodes[Index].Insert(Entity); //Store it into the Index node
                    return;
                }
                else if (Index == -1) // If the Value is -1
                {
                    foreach (int i in NodeLocations(Entity)) // Get the Nodes locations as a list
                    {
                        nodes[i].Insert(Entity);  // Store the entity in the Node which this Entity fits in
                        return;
                    }
                }
            }
            #endregion

            //======================================= If there are NO nodes ==============================================\\

            //Add the Entity to the list of Objects
            objects.Add(Entity);

            //======================================= If we CAN/NEED to create new Nodes ==============================================\\
            #region Can Create New Nodes

            List<IAsset> ToRemove = new List<IAsset>();
            //If the Objects list length is greater than the maximum number of objects
            // and the Level is lower than Max levels
            if (objects.Count() > maxObjects && level < maxLevels)
            {
                if (nodes[0] == null) //if there are no nodes
                {
                    Split(); // Call the split method, creaing Sub Nodues
                }
                

                for (int i = 0; i < objects.Count; i++)  //For every Entity in the Objects list
                {
                    {
                        int Index = GetIndex(objects[i]); // Get the index Values of the entities
                        if (Index != -1) // If the Index isn't -1
                        {
                            nodes[Index].Insert(objects[i]); //Call the insert Method of the subnode
                        }
                        else if (Index == -1) // For every Object
                        {
                            foreach (int x in NodeLocations(objects[i])) // Get the Nodes locations as a list
                            {
                                nodes[x].Insert(objects[i]); // Store the entity in the Node which this Entity fits in
                            }
                        }
                        ToRemove.Add(objects[i]); //Remove the entity at point I  in the Objects list
                    }

                }
            }

            foreach (IAsset ent in ToRemove)
            {
                objects.Remove(ent);
            }
            #endregion
        }

        /// <summary>
        /// Find out what entites share a node together
        /// </summary>
        /// <param name="returnObjects"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public List<IAsset> Retrieve(List<IAsset> returnObjects, IAsset Entity)
        {
            //List<IAsset> retObj  = new List<IAsset>();

            int Index = GetIndex(Entity); // Index is the index value of the entity
                                          // Console.WriteLine(Entity.getTag() + " Is on Level " + level + " : The Index Value is " + Index);

            if (Index != -1 && nodes[0] != null) //If the index isn't -1
            {
                nodes[Index].Retrieve(returnObjects, Entity); // Call retrieve method from the node which holds this entity
            }
            else if (Index == -1 && nodes[0] != null)      //If there are nodes below this quad and index = -1
            {
                foreach (QuadTree node in nodes) //For every node in the Nodes List
                {
                    node.Retrieve(returnObjects, Entity); // Call the retrieve method for each Node
                }
            }

            foreach (IAsset ent in objects)     // For each Asset in this Node / Quad
            {
                if (ent != Entity)             //If the item isn't the one being passed to this method
                    returnObjects.Add(ent);    //Add the asset to the return Objects list
            }
            returnObjects = returnObjects.Distinct().ToList();
            return returnObjects; // Return the return Objects list
        }


        /// <summary>
        /// Get the Node value of the Entity.
        /// 0 - Top Left : 1 - Top Right : 2 - Bottom Left : 3 - Bottom Right
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int GetIndex(IAsset Entity)
        {
            int Index = -1; //Base Index Value. -1 reperesents Parent Node

            #region Bool Conditions


            //If an Entities Y pos is Less than the center Y point, its in the Top Quads
            bool topQuads = (Entity.Position.Y < YMidpoint && //If the entity is greater than Y MidPoint
                Entity.Position.Y + Entity.Texture.Height < YMidpoint);  //If the entity height is less than the Y midpoint

            //If an Entities Y pos is Greater than the center Y point, its in the Bottom Quads
            bool bottomQuads = (Entity.Position.Y > YMidpoint && //If the entity is greater than Y MidPoint
                Entity.Position.Y + Entity.Texture.Height > YMidpoint);//< bounds.Y + bounds.Height); //If the entity is within the quadtree bounds

            //If an Entities x pos is Less than the center X point, its in the Left Quads
            bool leftQuads = (Entity.Position.X < XMidpoint && // iF ENTITY is less than X Midpoint
                Entity.Position.X + Entity.Texture.Width < XMidpoint); // If the entities width is less than the midpoint

            //If an Entities x pos is Less than the center X point, its in the Right Quads
            bool rightQuads = (Entity.Position.X > XMidpoint && //If the entity is greater than X MidPoint
                Entity.Position.X + Entity.Texture.Width > XMidpoint);//< bounds.X + bounds.Width); //If the entity is within the quadtree bounds

            #endregion

            //Set the Indx values based on the position of the entity in relation to the quadrants
            if (topQuads && leftQuads)
            { Index = 0; }                                //0 = Top Left

            else if (topQuads && rightQuads)
            { Index = 2; }                                //1 = Top Right

            else if (bottomQuads && leftQuads)
            { Index = 1; }                               //2 =  Bottom Left

            else if (bottomQuads && rightQuads)
            { Index = 3; }                                //3  =bottom Right            


            return Index;   //Return the INdex Value
        }

        /// <summary>
        /// Returns the list of Nodes and Entity Fits in
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        private List<int> NodeLocations(IAsset Entity)
        {
            List<int> NodeList = new List<int>(); //New list to store the index values, reperesenting the nodes which hold this entity

            #region Bool Conditions
            //If The Entity is in the Top Right
            bool topRight = (Entity.Position.X + Entity.Texture.Width > XMidpoint &&
                            Entity.Position.Y < YMidpoint);// || Entity.Position.X + Entity.Texture.Width > XMidpoint && Entity.Position.Y < YMidpoint);
            //If the entity is in the Top Left Quad
            bool topLeft = (Entity.Position.X < XMidpoint && 
                            Entity.Position.Y < YMidpoint);
            //If the Bottom Right of the entity is in the Bottom Right
            bool BottomRight = (Entity.Position.X + Entity.Texture.Width > XMidpoint &&
                                Entity.Position.Y + Entity.Texture.Height > YMidpoint);
            //If the Entity is in the bottom Left
            bool BottomLeft = (Entity.Position.X < XMidpoint &&
                                Entity.Position.Y + Entity.Texture.Height > YMidpoint);

            #endregion

            //Store the Index values based on the position of the entity in relation to the quadrants
            if (topLeft)
            { NodeList.Add(0); }                               //0 = Top Left

            if (BottomLeft)
            { NodeList.Add(1); }                               //1 = Bottom Left

            if (topRight)
            { NodeList.Add(2); }                               //2 = Top Right

            if (BottomRight)
            { NodeList.Add(3); }                               //3  = bottom Right

            return NodeList; //Return the NodeList Holding the Nodes which the entity is to be held in
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
