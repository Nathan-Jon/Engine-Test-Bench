using System;
using System.Collections.Generic;

namespace StateMachine.StateMachine
{
    public class StateMachine<T> : IStateMachine
    {
        #region Variables

        private IDictionary<Type, IState> states;

        private Type currentState;

        private T Entity;


        #endregion

        #region Methods

        public StateMachine(T entity)
        {
            this.Entity = entity;
            currentState = null;
            states = new Dictionary<Type, IState>();
        }

        public void AddState(IState state)
        {
            if (states.Count == 0)
            {
                currentState = state.GetType();
                state.Enter(Entity);
            }

            if (!CheckState(state.GetType()))
            {
                states.Add(state.GetType(), state);
            }

        }

        public void Update()
        {

            states[currentState].Update(Entity);
        }


        private bool CheckState(Type State)
        {
            return states.ContainsKey(State);
        }

        private void ChangeState(Type changeto)
        {
            if (changeto != null)
            {
                states[currentState].Exit(Entity);
                currentState = changeto;
                states[currentState].Enter(Entity);
            }
        }

        #endregion

    }
}
