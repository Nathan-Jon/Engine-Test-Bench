using DemonstrationEngine.Collision_Management;
using DemonstrationEngine.Physics;
using Microsoft.Xna.Framework;

namespace DemonstrationEngine
{
    class Square : PhysicsObject, ICollidable
    {
        public float ForceX = 1;
        public float ForceY = 2;
    

        public void Move()
        {
            ApplyForce(new Vector2(-ForceX, 0));
        }

        public void CollisionDetection()
        {
            //Side Of Screen//

            if (Position.X >= 850)
            {
                Position = (new Vector2(850, Position.Y));
                ForceX = ForceX * -1;

            }
            if (Position.X <= 0)
            {
                Position = (new Vector2(0, Position.Y));
                ForceX = ForceX * -1;
            }
            if (Position.Y >= 550)
            {
                Position = (new Vector2(Position.X, 550));
            }
            if (Position.Y <= 0)
            {
                Position = (new Vector2(Position.X, 0));
            }

        }

        public override void Update()
        {
            HitBox = new Rectangle((int)Position.X - 25, (int)Position.Y - 25, Texture.Width * 2, Texture.Height * 2);
            // move();
            CollisionDetection();
            UpdatePhysics();
        }
        
    }
}
