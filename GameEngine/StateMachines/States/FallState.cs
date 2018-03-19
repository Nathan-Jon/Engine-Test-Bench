using System;
using DemonstrationEngine.Physics;
using Microsoft.Xna.Framework;

namespace DemonstrationEngine.StateMachines.States
{
    class FallState<T> : IState<T> where T : IPhysics
    {

        public bool success { get; }

        public void Enter(T entity)
        {
            entity.GravityBool = true;
            Console.WriteLine("Successfully entered Fall State");

        }

        public void Update(T entity)
        {
            entity.ApplyForce(new Vector2(0, -5));
        }

        public void Exit(T entity)
        {    }
    }
}
