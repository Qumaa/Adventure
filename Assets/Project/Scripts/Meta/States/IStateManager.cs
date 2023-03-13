using System;

namespace Project.States
{
    public interface IStateManager
    {
        void TickStates();
        void AssignDefaultState(IState state);
        void RegisterState(IState state);
        void AddTransition(IState from, IState to, Func<bool> condition);
        void AddAnyTransition(IState to, Func<bool> condition);
    }
}
