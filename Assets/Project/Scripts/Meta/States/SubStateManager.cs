namespace Project.States
{
    public sealed class SubStateManager : StateManager, IState
    {
        public void Enter()
        {
            base.SetState(base.DefaultState);
        }

        public void Exit()
        {
            base.CurrentState?.Exit();
            base.UnsafeClearCurrentState();
        }

        public void Update()
        {
            base.TickStates();
        }
    }
}
