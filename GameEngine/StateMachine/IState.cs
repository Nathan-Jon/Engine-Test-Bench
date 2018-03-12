using System;

namespace DemonstrationEngine.StateMachine
{
    public interface IState
    {

        void OnEnter(IAsset ent);
        void OnUpdate(IAsset ent);
        void OnExit(IAsset ent);

    }
}