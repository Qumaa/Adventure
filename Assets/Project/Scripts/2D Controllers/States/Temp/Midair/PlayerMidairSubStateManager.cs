using UnityEngine;

namespace Project.Controller2D.Player
{
    public class PlayerMidairSubStateManager : PlayerSubStateManager
    {
        public PlayerMidairSubStateManager(EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(entityData, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _entityData.HandlerFacade.UpdateNormal(Vector2.up);
        }

        public override void Exit()
        {
            base.Exit();
            _entityData.HandlerFacade.ResetAccumulatedGravity();
        }
    }
}