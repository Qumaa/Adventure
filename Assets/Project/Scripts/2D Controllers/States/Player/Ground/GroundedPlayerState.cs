namespace Project.Controller2D.Player
{
    public abstract class GroundedPlayerState : PlayerState
    {
        protected GroundedPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
            
        }

        public override void Enter()
        {
            UpdateNormal();
            _entityData.Sensor.OnDataChanged += OnDataChangedHandle;
        }

        public override void Update()
        {
            UpdateNormal();
            
            base.Update();
        }

        protected virtual void UpdateNormal()
        {
            var groundFilter = _entityData.Sensor.Filters.Ground;
            
            _entityData.HandlerFacade.UpdateNormal(groundFilter);
        }

        public override void Exit()
        {
            _entityData.Sensor.OnDataChanged -= OnDataChangedHandle;
        }

        protected void OnDataChangedHandle() => UpdateNormal();
    }
}