using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine.StateMachine
{
    /// <summary>
    /// Interface required for all State Classes
    /// </summary>
    public interface IState
    {
        void Enter<T>(T Entity);    //Called Upon Entrance of State
        void Update<T>(T Entity);   //Update method of state
        void Exit<T>(T Entity);     //Called upon Exiting of state
    }
}
