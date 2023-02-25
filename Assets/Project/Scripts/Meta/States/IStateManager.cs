namespace Project.States
{
    public interface IStateManager
    {
        IState CurrentState { get; }

        public void TickStates();
    }
}
