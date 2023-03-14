using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.States
{
    public class StateManager : IStateManager
    {
        #region Values

        private const string _STATE_CANNOT_BE_NULL = "State cannot be null";
        private const string _DEFAULT_STATE_IS_NOT_ASSIGNED = "Default state is not assigned";
        private const string _TRIED_TO_ADD_NULL = "Tried to add null";
        private const string _DUPLICATE_ITEM_TRIED_TO_BE_ADDED = "Duplicate item tried to be added";

        protected IState _defaultState { get; private set; }

        private IState _currentState;
        private List<IState> _statesRegistry;
        private Dictionary<IState, List<Transition>> _transitionsRegistry;
        private List<Transition> _anyTransitionsRegistry;

        private Transition[] _currentStateTransitions;

        public StateManager()
        {
            _defaultState = _currentState = null;

            _statesRegistry = new List<IState>();
            _transitionsRegistry = new Dictionary<IState, List<Transition>>();
            _anyTransitionsRegistry = new List<Transition>();

            _currentStateTransitions = null;
        }

        #endregion

        #region Interface

        public IState CurrentState => _currentState;

        public void TickStates()
        {
            CheckNullState();
            
            CheckTransitions();
            
            _currentState?.Update();
        }

        public IStateManager RegisterDefaultState(IState state)
        {
            RegisterState(state);
            _defaultState = state;

            return this;
        }

        public IStateManager RegisterState(IState state)
        {
            PreventNull(state, _STATE_CANNOT_BE_NULL);
            
            SafeAddToCollection(state, _statesRegistry);
            return this;
        }

        public IStateManager AddTransition(IState from, IState to, Func<bool> condition)
        {
            PreventNull(to, _STATE_CANNOT_BE_NULL);
            PreventNull(from, _STATE_CANNOT_BE_NULL);
            
            SafeAddToCollection(new Transition(to, condition), GetTransitionsInternal(from));
            return this;
        }

        public IStateManager AddBidirectionalTransition(IState from, IState to, Func<bool> condition) => 
            AddTransition(from, to, condition).AddTransition(to, from, () => !condition());

        public IStateManager AddAnyTransition(IState to, Func<bool> condition)
        {
            SafeAddToCollection(new Transition(to, condition), _anyTransitionsRegistry);
            return this;
        }

        #endregion

        #region Internal logic

        private void SafeAddToCollection<TItem>(TItem item, ICollection<TItem> collection)
        {
            if (collection.Contains(PreventNull(item, _TRIED_TO_ADD_NULL)))
                throw new Exception(_DUPLICATE_ITEM_TRIED_TO_BE_ADDED);
            
            collection.Add(item);
        }

        private bool CheckTransitions() => 
            CheckTransitions(_anyTransitionsRegistry) || CheckTransitions(_currentStateTransitions);

        private List<Transition> GetTransitionsInternal(IState from)
        {
            if (from == null)
                return default;

            if (!_transitionsRegistry.ContainsKey(from)) 
                _transitionsRegistry[from] = new List<Transition>();

            return _transitionsRegistry[from];
        }

        private void CheckNullState()
        {
            if (_currentState == null)
                SetDefaultState();
        }

        private T PreventNull<T>(T item, string message)
        {
            if (item == null)
                throw new Exception(message);

            return item;
        }

        #endregion

        #region Protected logic

        protected Transition[] GetTransitions(IState from) => 
            GetTransitionsInternal(from).ToArray();
        protected Transition[] GetAnyTransitions() => 
            _anyTransitionsRegistry.ToArray();

        protected bool CheckTransitions(ICollection<Transition> trans)
        {
            if (trans == null) 
                return false;
                
            foreach (Transition tran in trans)
            {
                if (!tran.Condition()) 
                    continue;
                    
                SetState(tran.TargetState);
                return true;
            }

            return false;
        }

        protected IState[] GetAllStates() => 
            _statesRegistry.ToArray();

        protected void SetState(IState next)
        {
            if (_currentState == next) return;
            
            _currentState?.Exit();

            _currentState = PreventNull(next, _STATE_CANNOT_BE_NULL);
            _currentStateTransitions = GetTransitions(_currentState);
            
            _currentState.Enter();
        }

        protected void SetDefaultState() =>
            SetState(PreventNull(_defaultState, _DEFAULT_STATE_IS_NOT_ASSIGNED));

        #endregion

        #region Transition class

        protected class Transition
        {
            public readonly IState TargetState;
            public readonly Func<bool> Condition;

            public Transition(IState targetState, Func<bool> condition)
            {
                TargetState = targetState;
                Condition = condition;
            }
        }

        #endregion
    }
}
