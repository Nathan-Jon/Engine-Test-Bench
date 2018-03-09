using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine.StateMachine
{
    /// <summary>
    /// Interface for State Machine. Used for Adding States an calling the Update
    /// </summary>
    public interface IStateMachine<T>
    {
        void AddState(IState<T> state, string id); //Add States to the state Machine Dictionary
        void Update();  //Update Methods

    }
}
