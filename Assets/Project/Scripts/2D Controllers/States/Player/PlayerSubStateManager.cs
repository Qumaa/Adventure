using Project.States;

namespace Project.Controller2D.Player
{
    public abstract class PlayerSubStateManager : SubStateManager
    {
        protected readonly EntityController2DData<IGroundSensorPlayer> _entityData;
        protected readonly PlayerController2DData _playerData;

        public PlayerSubStateManager(EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData)
        {
            _entityData = entityData;
            _playerData = playerData;
        }
    }
}