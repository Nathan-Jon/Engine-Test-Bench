using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    interface IAsset
    {
        void setPoints();
        List<Vector2> getPoints();
        List<Vector2> getAxies();

    }
}
