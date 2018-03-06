using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine.StateMachine
{
    public interface IStateMachine
    {
        void AddState(IState state);

        void Update();

    }
}
