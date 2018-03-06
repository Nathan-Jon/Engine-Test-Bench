using System;
using System.Collections.Generic;

namespace StateMachine.StateMachine
{
    /// <summary>
    /// State Machines Handle used to store and manage different State classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateMachine<T> : IStateMachine
    {
        #region Variables

        //Dictionary to hold the different State classes
        private IDictionary<Type, IState> States;

        //Generic Type ActiveState
        private Type ActiveState;

        //To Store the entity which this state machine belongs to
        private T Entity;


        #endregion

        #region Methods

        /// <summary>
        /// Construtor for the State Machine, Requires the Entity this state machine belongs to
        /// </summary>
        /// <param name="entity"></param>
        public StateMachine(T entity)
        {
            //Initialise Variables
            this.Entity = entity;
            ActiveState = null;
            States = new Dictionary<Type, IState>();
        }

        /// <summary>
        /// Add States to the States Dictionary
        /// </summary>
        /// <param name="state"></param>
        public void AddState(IState state)
        {
            //If the Dictionary is empty
            if (States.Count == 0)
            {
                //The Active State is the State being passed
                ActiveState = state.GetType();
                //Call the States Enter Method
                state.Enter(Entity);
            }

            //If the Dictionary doesnt hold a copy of this State
            if (!HoldsState(state.GetType()))
            {
                //Add this State to the dictionary
                States.Add(state.GetType(), state);
            }

        }

        /// <summary>
        /// Update Method for State Machine
        /// </summary>
        public void Update()
        {
            //Update the stae in the dictionary of Type currentState
            States[ActiveState].Update(Entity);
        }


        /// <summary>
        /// Looks to see whether or not the Dictionary holds a State
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        private bool HoldsState(Type State)
        {
            //Returns the state in the dictionary based on the Key passed
            return States.ContainsKey(State); 
        }

        /// <summary>
        /// When called, Changes the Current State, calls the exit method and the enter method
        /// </summary>
        /// <param name="changeto"></param>
        private void ChangeState(Type changeto)
        {
            //If the type isnt null
            if (changeto != null)
            {
                //Caal the exit behaviour of the current state
                States[ActiveState].Exit(Entity);
                //Change the current to state to the Type being passed into the method
                ActiveState = changeto;
                //Call The Update method of the new currentState from the dictionary
                States[ActiveState].Enter(Entity);
            }
        }

        #endregion

    }
}
