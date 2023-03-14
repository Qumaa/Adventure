using System.Collections;
using System.Collections.Generic;

namespace Project.Controller2D.Player
{
    public class MovingGroundedPlayerState : SnappingGroundedPlayerState
    {
        public MovingGroundedPlayerState(Controller2DInputData inputData,
            EntityController2DData<IGroundSensorPlayer> entityData, PlayerController2DData playerData) : base(inputData,
            entityData, playerData)
        {
        }

        public override void Update()
        {
            Move();

            base.Update();
        }
    }
}