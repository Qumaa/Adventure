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

        public virtual void Update()
        {
            _entityData.HandlerFacade.ApplyInputFrame();
        }

        public abstract void Exit();

        protected void Brake()
        {
            float maxSpeed = _entityData.Settings.MaxSpeed;
            float dist = _entityData.Settings.DecelerationDistance;

            Brake(maxSpeed, dist);
        }
        private void Brake(float maxSpeed, float dist) => 
            _entityData.HandlerFacade.Brake(maxSpeed, dist);

        protected void Move()
        {
            float maxSpeed = _entityData.Settings.MaxSpeed;
            float dist = _entityData.Settings.AccelerationDistance;

            Move(maxSpeed, dist);
        }
        private void Move(float maxSpeed, float dist) => 
            _entityData.HandlerFacade.Move(_inputData.Movement, maxSpeed, dist);

        protected void AccumulateGravity() => 
            _entityData.HandlerFacade.AccumulateGravity();
        protected void ResetAccumulatedGravity() => 
            _entityData.HandlerFacade.ResetAccumulatedGravity();
    }
}