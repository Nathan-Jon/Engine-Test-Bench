using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GameEngine;

namespace StateMachine.StateMachine.States
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
