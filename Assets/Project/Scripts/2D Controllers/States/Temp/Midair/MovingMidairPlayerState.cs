namespace Project.Controller2D.Player
{
    public class MovingMidairPlayerState : MidairPlayerState
    {
        public MovingMidairPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        public override void Update()
        {
            Move();
            base.Update();
        }
    }
}