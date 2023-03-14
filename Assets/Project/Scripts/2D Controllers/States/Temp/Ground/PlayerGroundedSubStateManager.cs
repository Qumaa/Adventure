using UnityEngine;

namespace Project.Controller2D.Player
{
    public class PlayerGroundedSubStateManager : PlayerSubStateManager
    {
        public PlayerGroundedSubStateManager(EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(entityData, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _entityData.HandlerFacade.ResetAccumulatedGravity();
        }

        public override void Exit()
        {
            base.Exit();
            _entityData.HandlerFacade.UpdateNormal(Vector2.up);
        }
    }
}