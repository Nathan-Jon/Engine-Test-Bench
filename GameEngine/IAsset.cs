using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    interface IAsset
    {
        void setPoints();
        Texture2D getTex { get;}
        Vector2 getPos { get; }
        List<Vector2> getPoints();
        List<Vector2> getAxies();



    }
}
