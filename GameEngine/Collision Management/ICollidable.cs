using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemonstrationEngine.CollisionManagement
{
    public interface ICollidable
    {
        void hasCollisions(IAsset asset);
    }
}
