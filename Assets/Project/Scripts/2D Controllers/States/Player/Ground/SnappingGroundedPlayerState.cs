using UnityEngine;

namespace Project.Controller2D.Player
{
    public abstract class SnappingGroundedPlayerState : GroundedPlayerState
    {
        public SnappingGroundedPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        protected override void UpdateNormal()
        {
            var groundFilter = _entityData.Sensor.Filters.Ground;
            
            if (_entityData.Sensor.Ground)
            {
                base.UpdateNormal();
                return;
            }

            _entityData.HandlerFacade.SnapAndUpdateNormal(groundFilter, groundFilter, _entityData.Settings.SnapDistance);
        }
    }
}