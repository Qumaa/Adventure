using System;
using System.Collections.Generic;

namespace Project.States
{
    public class StateManager : IStateManager
    {
        IState _currentState;
        Type _defaultStateType;

        private Dictionary<Type, IState> _stateRegistry;
        private Dictionary<Type, List<Transition>> _stateTransitionsRegistry;
        private List<Transition> _anyTransitions;
        private List<Transition> _availableTransitions;

        public IState CurrentState => _currentState;

        public StateManager()
        {
            _stateRegistry = new Dictionary<Type, IState>();
            _stateTransitionsRegistry = new Dictionary<Type, List<Transition>>();
            _anyTransitions = new List<Transition>();
            _availableTransitions = new List<Transition>();
        }

        public virtual void TickStates()
        {
            if (_currentState == null) 
                GetStateFromRegistry(_defaultStateType, out _currentState);

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

        protected virtual void SetState(IState state)
        {
            if (_currentState == state) return;

            _currentState?.Exit();

            _currentState = state;
            _availableTransitions = GetTransitions(state);

            _currentState.Enter();
        }
        protected virtual void SetState<TState>() where TState : IState
        {
            if (GetStateFromRegistry<TState>(out IState result))
                SetState(result);
        }
        public virtual void SetDefaultState<TState>() where TState : IState
        {
            _defaultStateType = typeof(TState);
        }

        private List<Transition> GetTransitions(IState state)
        {
            Type key = state.GetType();
            if (!_stateTransitionsRegistry.ContainsKey(key)) _stateTransitionsRegistry.Add(key, new List<Transition>());

            return _stateTransitionsRegistry[key];
        }
        private bool GetStateFromRegistry(Type type, out IState result)
        {
            if (!_stateRegistry.TryGetValue(type, out result))
                throw new Exception($"No state of type {type} has been registered in {this}, but there was a request for it");

            return true;
        }
        private bool GetStateFromRegistry<TType>(out IState result) where TType : IState
        {
            return GetStateFromRegistry(typeof(TType), out result);
        }

        public void RegisterState(IState state)
        {
            // prevent adding duplicates
            if (_stateRegistry.ContainsKey(state.GetType()))
                throw new Exception($"{this} tried to add an existing state: {state}");

            _stateRegistry.Add(state.GetType(), state);
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
        public void AddTransition<TFrom, TTo>(Func<bool> condition) where TFrom : IState where TTo : IState
        {
            if (GetStateFromRegistry<TFrom>(out IState from) && GetStateFromRegistry<TTo>(out IState to))
                AddTransition(from, to, condition);
        }
        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            Transition newTransition = new Transition(to, condition);

            // prevent adding duplicates
            if (_anyTransitions.Contains(newTransition))
                throw new Exception($"{this} tried to add an existing any transition: {newTransition}");

            _anyTransitions.Add(newTransition);
        }
        public void AddAnyTransition<TTo>(Func<bool> condition) where TTo : IState
        {
            if (GetStateFromRegistry<TTo>(out IState to))
                AddAnyTransition(to, condition);
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
