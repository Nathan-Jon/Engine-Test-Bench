﻿using System;
using System.Collections.Generic;
using StateMachine.StateMachine.TransitionMethods;

namespace StateMachine.StateMachine
{
    /// <summary>
    /// State Machines Handle used to store and manage different State classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateMachine<T> : IStateMachine<T>
    {
        #region Variables

        //Dictionary to hold the different State classes
        private IDictionary<string, IState<T>> States;
        //Dictionary to hold the different Transition types
        private IDictionary<string, ITransitionHandler> Transitions;

        //Generic Type ActiveState
        private string ActiveState;

        //To Store the entity which this state machine belongs to
        private T Entity;


        #endregion


        #region StateManagement
        
        /// <summary>
        /// Construtor for the State Machine, Requires the Entity this state machine belongs to
        /// </summary>
        /// <param name="entity"></param>
        public StateMachine(T entity)
        {
            //Initialise Variables
            this.Entity = entity;
            ActiveState = null;
            States = new Dictionary<string, IState<T>>();
            Transitions = new Dictionary<string, ITransitionHandler>();
        }

        /// <summary>
        /// Add States to the States Dictionary
        /// </summary>
        /// <param name="state"></param>
        public void AddState(IState<T> state, string stateID)
        {
            //If the Dictionary is empty
            if (States.Count == 0)
            {
                //The Active State is the State being passed
                ActiveState = stateID;
                //Call the States Enter Method
                state.Enter(Entity);
            }

            //If the Dictionary doesnt hold a copy of this State
            if (!HoldingState(stateID))
            {
                //Add this State to the dictionary
                States.Add(stateID, state);
            }

        }

        /// <summary>
        /// Looks to see whether or not the Dictionary holds a State
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        private bool HoldingState(string State)
        {
            //Returns the state in the dictionary based on the Key passed
            return States.ContainsKey(State); 
        }

        /// <summary>
        /// When called, Changes the Current State, calls the exit method and the enter method
        /// </summary>
        /// <param name="changeto"></param>
        private void ChangeState(string changeto)
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


        /// <summary>
        /// Adds transitions between state that 
        /// </summary>
        /// <param name="methodVal"></param>
        /// <param name="stateFrom"></param>
        /// <param name="targetState"></param>
        public void AddMethodTransition(Func<bool> methodVal, string stateFrom, string targetState)
        {
            //Call the Method Tranisiton method with a default BOOl state of required result
            AddMethodTransition(methodVal, stateFrom, targetState, true); 
        }


        /// <summary>
        /// Store Method transition types in the Transitions dictionary
        /// </summary>
        /// <param name="MethodVal"></param>
        /// <param name="stateFrom"></param>
        /// <param name="targetState"></param>
        /// <param name="ReqResult"></param>
        public void AddMethodTransition(Func<bool> MethodVal, string stateFrom, string targetState, bool ReqResult)
        {
            //Check to see whether or not the dictionary holds both states and both states aren't the same
            isValidTransition(stateFrom, targetState);
            //Look to see whether or not the base transition state is currently held in the dictionary
            HoldingState(stateFrom);
            //Store the method transition in the transition dictionary
            Transitions[stateFrom].StoreMethodTransition(targetState, MethodVal, ReqResult);
        }

        /// <summary>
        /// returns a true statement id both of the States are held in the dictionary of states
        /// </summary>
        /// <param name="baseState"></param>
        /// <param name="targetState"></param>
        /// <returns></returns>
        private bool isValidTransition(string baseState, string targetState)
        {
            //Check to see if base state to target state is a valid transion
            if (States.ContainsKey(baseState) && States.ContainsKey(targetState) && baseState != targetState)
            {
                //The Transition is Valid
             return true;
            }
            //The transition is invalid
            return false;



        }

        /// <summary>
        /// Update Method for State Machine
        /// </summary>
        public void Update()
        {
            //Update the stae in the dictionary of Type currentState
            CheckMethodTransition();
            //Call the update method on the active state
            //States[ActiveState].Update(Entity);


        }



        public void CheckMethodTransition()
        {
            if(States.Keys.Contains(ActiveState))
                ChangeState((Transitions[ActiveState].CheckMethodTransition()));
        }

    }
}
