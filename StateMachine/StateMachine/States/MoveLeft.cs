using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine;
using Microsoft.Xna.Framework;

namespace StateMachine.StateMachine.States
{
    class MoveLeft<T> : IState<T> where T: IAsset 
    {
        public bool success { get; }

        public void Enter(T entity)
        {
            entity.ApplyForce(new Vector2(5,0));
        }

        public void Update(T entity)
        {
            entity.ApplyForce(new Vector2(5, 0));
        }

        public void Exit(T entity)
        {

        }
    }
}
