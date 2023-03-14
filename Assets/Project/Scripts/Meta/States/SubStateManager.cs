namespace Project.States
{
    public class SubStateManager : StateManager, IState
    {
        public virtual void Enter()
        {
            if (!CheckAllTransitions())
                SetDefaultState();
        }

        public virtual void Update()
        {
            TickStates();
        }

        public virtual void Exit()
        {
        }

        private bool CheckAllTransitions()
        {
            foreach (var state in GetAllStates())
            {
                if (CheckTransitions(GetTransitions(state))) 
                    return true;
            }

            return CheckTransitions(GetAnyTransitions());
        }
    }
}
