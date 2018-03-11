using Microsoft.Xna.Framework;

namespace DemonstrationEngine.Physics
{
    class Plane
    {
        private Vector2 v2Normal;
        private float offset;


        public Plane(Vector2 normal, float offset)
        {
            this.offset = offset; 
            v2Normal = normal;
            v2Normal.Normalize();
        }
    }
}
