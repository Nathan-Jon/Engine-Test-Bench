using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StateMachine.StateMachine;

namespace GameEngine
{
    public class TestPlayerState1<T> : IState<T> where T : IAsset
    {
        public bool success
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public void Enter<T>(T entity)
        {
            //entity.Position = new Vector2(400, 400);
        }

        public void Update<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public void Exit<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}