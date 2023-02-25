using System;
using System.Collections.Generic;

namespace Project.States
{
    public class StateManager : IStateManager
    {
        IState _currentState;
        IState _defaultState;

        private List<IState> _stateRegistry;
        private Dictionary<IState, List<Transition>> _stateTransitionsRegistry;
        private List<Transition> _anyTransitions;
        private List<Transition> _availableTransitions;

        public IState CurrentState => _currentState;
        protected IState DefaultState => _defaultState;

        public StateManager()
        {
            _stateRegistry = new List<IState>();
            _stateTransitionsRegistry = new Dictionary<IState, List<Transition>>();
            _anyTransitions = new List<Transition>();
            _availableTransitions = new List<Transition>();
        }

        public virtual void TickStates()
        {
            if (_currentState == null)
            {
                if (_defaultState == null) return;
                SetState(_defaultState);
            }

            CheckTransitions();

            _currentState.Update();
        }
        protected virtual void CheckTransitions()
        {
            foreach (Transition t in _anyTransitions)
            {
                if (t.Condition())
                {
                    SetState(t.TargetState);
                    return;
                }
            }

            foreach (Transition t in _availableTransitions)
            {
                if (t.Condition())
                {
                    SetState(t.TargetState);
                    return;
                }
            }
        }

        protected void SetState(IState state)
        {
            if (state == null || _currentState == state) return;

            _currentState?.Exit();

            _currentState = state;
            _availableTransitions = GetTransitions(state);

            _currentState.Enter();
        }

        internal void AssignDefaultState(IState state)
        {
            if (state == null) return;
            _defaultState = state;
        }

        private List<Transition> GetTransitions(IState state)
        {
            if (!_stateTransitionsRegistry.ContainsKey(state)) _stateTransitionsRegistry.Add(state, new List<Transition>());

            return _stateTransitionsRegistry[state];
        }

        public void RegisterState(IState state)
        {
            // prevent adding duplicates
            if (_stateRegistry.Contains(state))
                throw new Exception($"{this} tried to add an existing state: {state}");

            _stateRegistry.Add(state);
        }
        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            List<Transition> transitions = GetTransitions(from);
            Transition newTransition = new Transition(to, condition);

            // prevent adding duplicates
            if (transitions.Contains(newTransition))
                throw new Exception($"{this} tried to add an existing transition: {newTransition}");

            transitions.Add(newTransition);
        }
        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            Transition newTransition = new Transition(to, condition);

            // prevent adding duplicates
            if (_anyTransitions.Contains(newTransition))
                throw new Exception($"{this} tried to add an existing any transition: {newTransition}");

            _anyTransitions.Add(newTransition);
        }

        protected void UnsafeClearCurrentState()
        {
            _currentState = null;
        }

        protected class Transition
        {
            public IState TargetState { get; private set; }
            public Func<bool> Condition { get; private set; }

            public Transition(IState to, Func<bool> condition)
            {
                TargetState = to;
                Condition = condition;
            }
        }        
    }
}
