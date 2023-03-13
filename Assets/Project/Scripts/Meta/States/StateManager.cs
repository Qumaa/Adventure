using System;
using System.Collections.Generic;

namespace Project.States
{
    public class StateManager : IStateManager
    {
        #region Values

        protected IState _defaultState { get; private set; }

        private IState _currentState;
        private List<IState> _statesRegistry;
        private Dictionary<IState, List<Transition>> _transitionsRegistry;
        private List<Transition> _anyTransitionsRegistry;

        private Transition[] _currentStateTransitions;

        public StateManager()
        {
            _defaultState = null;
        }

        #endregion

        #region Interface

        public void TickStates()
        {
            CheckNullState();
            
            CheckTransitions();
            
            _currentState?.Update();
        }

        public void AssignDefaultState(IState state) => 
            _defaultState = state;

        public void RegisterState(IState state) => 
            SafeAddToCollection(state, _statesRegistry);

        public void AddTransition(IState from, IState to, Func<bool> condition) =>
            SafeAddToCollection(new Transition(to, condition), GetTransitions(from));
        public void AddAnyTransition(IState to, Func<bool> condition) =>
            SafeAddToCollection(new Transition(to, condition), _anyTransitionsRegistry);

        #endregion

        private void SafeAddToCollection<TItem>(TItem item, ICollection<TItem> collection)
        {
            if (collection.Contains(item))
                throw new Exception($"Duplicate {item.ToString()} is tried to be added to {collection.ToString()}");
            
            collection.Add(item);
        }

        private List<Transition> GetTransitions(IState from)
        {
            if (from == null) return default;

            if (!_transitionsRegistry.ContainsKey(from))
                //throw new Exception($"No transitions registered for {from.ToString()}. Dead end?");
                return default;

            return _transitionsRegistry[from];
        }

        protected void SetState(IState next)
        {
            if (_currentState == next) return;
            
            _currentState?.Exit();

            _currentState = next;
            _currentStateTransitions = GetTransitions(_currentState).ToArray();
            
            _currentState?.Enter();
        }

        private void CheckNullState() =>
            _currentState ??= _defaultState ?? throw new Exception("Default state is not assigned");

        private void CheckTransitions()
        {
            if (Check(_anyTransitionsRegistry)) return;
            Check(_currentStateTransitions);
            
            bool Check(ICollection<Transition> list)
            {
                if (list == null) 
                    return false;
                
                foreach (Transition tran in list)
                {
                    if (!tran.Condition()) continue;
                    
                    SetState(tran.TargetState);
                    return true;
                }

                return false;
            }
        }

        private class Transition
        {
            public readonly IState TargetState;
            public readonly Func<bool> Condition;

            public Transition(IState targetState, Func<bool> condition)
            {
                TargetState = targetState;
                Condition = condition;
            }
        }
    }
}
