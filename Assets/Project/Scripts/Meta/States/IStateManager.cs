using System;

namespace Project.States
{
    public interface IStateManager
    {
        IState CurrentState { get; }
        void TickStates();
        IStateManager RegisterDefaultState(IState state);
        IStateManager RegisterState(IState state);
        IStateManager AddTransition(IState from, IState to, Func<bool> condition);
        /// <summary>
        /// Adds a transition from one state to another with specified condition
        /// and in reverse order with inverted condition 
        /// </summary>
        IStateManager AddBidirectionalTransition(IState from, IState to, Func<bool> condition);
        IStateManager AddAnyTransition(IState to, Func<bool> condition);
    }
}
