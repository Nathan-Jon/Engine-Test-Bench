using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    class QuadTreeManager
    {

        public List<QuadTree> quadList = new List<QuadTree>();

        public void addToList(QuadTree quad)
        {
            quadList.Add(quad);
        }

        public List<QuadTree> getQuadList()
        {
            return quadList;
        }

        public void DrawList(SpriteBatch sprite)
        {
            for (int i = 0; i < quadList.Count; i++)
            {
                if (quadList[0]!= null)
                {
                    quadList[i].Draw(sprite);
                }
            }
        }


    }
}
