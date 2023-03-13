using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Controller2D
{
    public class GodlikeGroundState : GroundedPlayerState
    {
        public GodlikeGroundState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        protected override void InheritorUpdate()
        {
            float maxSpeed = _entityData.Settings.MaxSpeed;
            float dist = _entityData.Settings.AccelerationDistance;
            float brakeDist = _entityData.Settings.DecelerationDistance;
            
            if (_inputData.Movement != 0)
            {
                _entityData.HandlerFacade.Move(_inputData.Movement, maxSpeed, dist);
            }
            else
            {
                _entityData.HandlerFacade.Brake(maxSpeed, brakeDist);
            }
        }
    }
}