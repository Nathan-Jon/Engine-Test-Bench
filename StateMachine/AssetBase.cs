using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics;

namespace GameEngine
{
    class AssetBase
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Acceleration;
        public Vector2 Gravity { get; set; }

        public bool GravityBool = true;
        public void SetGravity(bool setG)
        {
            GravityBool = setG;
        }

        //Inverse mass to encourage multiplication and not diviision due to multiplication being faster
        public float InverseMass =  -1.5f;
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
            if(GravityBool)
            { Gravity = new Vector2(0,5);}
            else
            {
                Gravity = new Vector2(0,-5);
            }
            Acceleration = Gravity;
        }

    }
}
