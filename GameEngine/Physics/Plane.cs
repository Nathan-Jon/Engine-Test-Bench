using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DemonstrationEngine.Physics
{
    class PlaneClass : IPlane//IAsset, IPlane
    {

        public Vector2 Normal { get; set; }
        public float Offset { get; set; }


        public PlaneClass(Vector2 normal, float offset)
        {
            Offset = offset;
            Normal = normal;
        }

        #region Interface
        //public Vector2 Position
        //{

        //}

        //public string Tag
        //{

        //}

        //public Texture2D Texture
        //{
            
        //}

        //public void BuildEdges()
        //{
        //}

        //public Vector2 Center()
        //{
        //}

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //}

        //public List<Vector2> Edges()
        //{
        //}

        //public List<Vector2> Point()
        //{
        //}

        //public void SetPoints()
        //{
        //}

        //public void Update()
        //{
        //}

#endregion
    }
}
