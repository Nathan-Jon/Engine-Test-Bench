using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DemonstrationEngine;
using GameEngine;

namespace StateMachine.StateMachine.States
{
    class JumpState<T>: IState<T> where T : IAsset
    {

        public bool success { get; }

        public void Enter(T entity)
        {
            entity.SetGravity(false);
            Console.WriteLine("SUCCESS ON ENTERING JUMP STATE");
        }

        public void Update(T entity)
        {
          //  throw new NotImplementedException();
        }

        public void Exit(T entity)
        {
            Console.WriteLine("Leaving Jump state");

        }
    }
}
