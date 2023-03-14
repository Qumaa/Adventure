using UnityEngine;

namespace Project.Controller2D.Player
{
    public class SlidingPlayerState : GroundedPlayerState
    {
        private float _accumulatedHorizontalVelocity;
        
        public SlidingPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _accumulatedHorizontalVelocity = 0;
        }

        public override void Update()
        {
            AccumulateGravity();
            _accumulatedHorizontalVelocity = ExtractHorizontalVelocity();
            
            base.Update();
        }

        public override void Exit()
        {
            _entityData.HandlerFacade.Handler.HorizontalVelocity = _accumulatedHorizontalVelocity;
            ResetAccumulatedGravity();
        }

        private float ExtractHorizontalVelocity()
        {
            var dir = _entityData.HandlerFacade.Handler.NormalRight;
            var vel = _entityData.HandlerFacade.Handler.WorldVelocity;
            return Vector2.Dot(dir, vel);
        }
    }
}