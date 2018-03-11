﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine;

namespace StateMachine.StateMachine
{
    /// <summary>
    /// Interface required for all State Classes
    /// </summary>
    public interface IState<T>
    {
        bool success { get; }

        void Enter(T entity);    //Called Upon Entrance of State
        void Update(T entity);   //Update method of state
        void Exit(T entity);     //Called upon Exiting of state
    }
}
