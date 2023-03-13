using System.Collections;
using System.Collections.Generic;
using Project.States;
using UnityEngine;

namespace Project.Controller2D
{
    public abstract class PlayerState : IState
    {
        protected readonly Controller2DInputData _inputData;
        protected readonly EntityController2DData<IGroundSensorPlayer> _entityData;
        protected readonly PlayerController2DData _playerData;

        protected PlayerState(Controller2DInputData inputData, EntityController2DData<IGroundSensorPlayer> entityData,
            PlayerController2DData playerData)
        {
            _inputData = inputData;
            _entityData = entityData;
            _playerData = playerData;
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();
    }
}