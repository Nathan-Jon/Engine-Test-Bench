using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine.StateMachine
{
    public interface IState
    {
        void Enter<T>(T Entity);
        void Update<T>(T Entity);
        void Exit<T>(T Entity);
    }
}
