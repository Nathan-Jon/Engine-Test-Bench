using System;

namespace DemonstrationEngine.StateMachines.States
{
    class FallState<T> : IState<T> where T : IAsset
    {

        public bool success { get; }

        public void Enter(T entity)
        {
            entity.SetGravity(true);
            Console.WriteLine("Successfully entered Fall State");

        }

        public void Update(T entity)
        {}

        public void Exit(T entity)
        {    }
    }
}
