namespace Project.States
{
    public class SubStateManager : StateManager, IState
    {
        public virtual void Enter()
        {
            SetState(_defaultState);
        }

        public virtual void Update()
        {
            TickStates();
        }

        public virtual void Exit()
        {
            SetState(null);
        }
    }
}
