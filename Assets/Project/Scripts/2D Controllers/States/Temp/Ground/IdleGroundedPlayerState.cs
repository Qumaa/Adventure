namespace Project.Controller2D.Player
{
    public class IdleGroundedPlayerState : SnappingGroundedPlayerState
    {
        public IdleGroundedPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        public override void Update()
        {
            Brake();

            base.Update();
        }
    }
}