using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DemonstrationEngine.StateMachine
{
    /// <summary>
    /// State Machine - Controls States 
    /// 
    /// Author: Nathan Robertso & Carl Chalmers
    /// Date : 09/03/18 Version 0.1
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateMachine<T>
    {
        private Dictionary<string, IState> States;
        private string ActiveState { get; set; }
        private IAsset entity;

        public StateMachine(IAsset _entity)
        {
            States = new Dictionary<string, IState>();
            ActiveState = null;
            entity = _entity;
        }

        /// <summary>
        /// Add States to the Dictionary of states
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="state"></param>
        public void AddState(string ID, IState state)
        {
            if (States[ID] == null)
                States.Add(ID, state);
            else
                throw new Exception(string.Format("Unable to add the State becuase state KEY : " + ID + " Already exists"));
        }

        /// <summary>
        /// Remove states from the dictionary of states
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveState(string ID)
        {
            if (States[ID] != null)
                States.Remove(ID);
            else
                throw new Exception(string.Format("Unable to remove State from Dictionary as " + ID + " doesn't exist"));
        }

        /// <summary>
        /// Change the current Active state, calling the exit method of the previous state and then the enter method of the new state
        /// </summary>
        /// <param name="ID"></param>
        public void ChangeState(string ID)
        {
            //Throw exception if the dictionary doesn't hold the required Key
            if (!States.ContainsKey(ID))
                throw new Exception(string.Format("Can't change to state of KEY : " + ID + " as Key does not exist"));
            else if (States.ContainsKey(ID))
            {
                //If there is currently a state
                if (ActiveState != null)
                    States[ActiveState].OnExit(entity); // Call the Exit Method

                ActiveState = ID; //Change the current State value
                States[ActiveState].OnEnter(entity); //Call the Enter Method of the new activeState
            }
        }

        public void Update()
        {
            //Call the Update Method of the active state
            States[ActiveState].OnUpdate(entity);
        }
    }
}