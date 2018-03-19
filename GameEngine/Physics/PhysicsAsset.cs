using Microsoft.Xna.Framework;

namespace DemonstrationEngine.Physics
{

    class PhysicsObject : AssetBase, IPhysics
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Gravity { get; set; }

        public bool GravityBool { get; set; }

        //Inverse mass to encourage multiplication and not diviision due to multiplication being faster
        public float InverseMass = -1.5f;

        public float Restitution = 1f;
        public float Damping = 0.5f;



        public void ApplyForce(Vector2 force)
        {
            //Multiply force by the inversemass to obtain the acceleration value
            Acceleration += force * InverseMass;
        }

        public void ApplyImpulse(Vector2 closingVelo)
        {
            //Apply Impulse by setting the velocy to the closing velocity by the Restitution of the entity
            Velocity = closingVelo * Restitution;
        }

        public void UpdatePhysics()
        {
            Velocity += Acceleration;
            Velocity *= Damping;
            Position += Velocity;

            //Apply Gravity
            if (GravityBool)
            {
                Gravity = new Vector2(0, 5);
            }
            else
            {
                Gravity = new Vector2(0, -5);
            }
            Acceleration = Gravity;

        }
    }
}