using UnityEngine;

namespace Project.Controller2D.Player
{
    public abstract class MidairPlayerState : PlayerState
    {
        protected MidairPlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData, entityData, playerData)
        {
        }

        public override void Enter()
        {
        }

        public override void Update()
        {
            AccumulateGravity();
            LimitFallingVelocity();
            
            base.Update();
        }

        public override void Exit()
        {
        }

        private void LimitFallingVelocity()
        {
            var limit = -_playerData.Settings.FallVelocityLimit;
            var velocity = _entityData.HandlerFacade.Handler.VerticalVelocity;
            _entityData.HandlerFacade.Handler.VerticalVelocity = Mathf.Max(velocity, limit);
        }
    }
}