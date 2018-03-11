using Microsoft.Xna.Framework;

namespace DemonstrationEngine.StateMachines.States
{
    class MoveRight<T> : IState<T> where T: IAsset 
    {
        public bool success { get; }

        public void Enter(T entity)
        {
            entity.ApplyForce(new Vector2(-5, 0));
        }

        public void Update(T entity)
        {
            entity.ApplyForce(new Vector2(-5, 0));

        }

        public void Exit(T entity)
        {

        }
    }
}
