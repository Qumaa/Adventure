using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Controller2D
{
    public abstract class GroundedPlayerState : PlayerState
    {
        public GroundedPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        public override void Enter()
        {
            _entityData.HandlerFacade.Handler.VerticalVelocity = 0;
            UpdateNormal();
        }

        public override void Update()
        {
            InheritorUpdate();

            UpdateNormal();

            float maxSpeed = _entityData.Settings.MaxSpeed;
            _entityData.HandlerFacade.LimitHorizotalVelocity(maxSpeed);
            
            Debug.DrawRay(_entityData.Capsule.bounds.center, _entityData.HandlerFacade.Handler.Normal * 2, Color.yellow);

            _entityData.HandlerFacade.ApplyInputFrame();
        }

        protected abstract void InheritorUpdate();

        public override void Exit()
        {
            _entityData.HandlerFacade.UpdateNormal(Vector2.up);
        }

        private void UpdateNormal()
        {
            var groundFilter = _entityData.Sensor.Filters.Ground;
            
            if (_entityData.Sensor.Ground)
            {
                _entityData.HandlerFacade.UpdateNormal(groundFilter);
                return;
            }

            _entityData.HandlerFacade.SnapAndUpdateNormal(groundFilter, groundFilter, _entityData.Settings.SnapDistance);
        }
    }
}