using System;
using UnityEngine;
using Project.States;

namespace Project.Controller2D
{
    public abstract class EntityController2D<TGroundSensor> : MonoBehaviour where TGroundSensor : IGroundSensor
    {
        #region Values

        // Editor values
        [SerializeField] private EntityController2DSettings _entitySettings;

        // Internal values
        protected EntityController2DData<TGroundSensor> EntityData { get; private set; }
        protected Controller2DInputData InputData { get; private set; }
        #endregion

        #region Controller logic


        protected virtual void Awake() => CreateData();
        protected virtual void FixedUpdate()
        {
            EntityData.StateManager.TickStates();
        }

        protected void BodyUpdate()
        {
            if (FallingCondition())
                EntityData.HandlerFacade.Handler.AccumulateVerticalVelocity();

            UpdateNormal();
            EntityData.HandlerFacade.Handler.UpdateWorldVelocity();
        }

        protected abstract bool FallingCondition();
        protected abstract void UpdateNormal();

        /// <summary>
        /// This method doesn't stand for jumping, but rather for processing and caching the input
        /// </summary>
        public virtual void Jump() => InputData.GiveJumpInput();
        /// <summary>
        /// This method doesn't stand for moving, but rather for processing and caching the input
        /// </summary>
        public virtual void Move(int moveDirectionSign) => InputData.GiveMoveInput(moveDirectionSign);
        // float overload
        public void Move(float moveDirectionSign) => Move(Mathf.RoundToInt(moveDirectionSign));

        private void CreateData()
        {
            if (EntityData != null)
                return;

            InputData = new Controller2DInputData();

            var capsule = this.TryGetReference<CapsuleCollider2D>();
            var stateManager = new StateManager();
            var groundSensor = this.TryGetReference<TGroundSensor>();
            var rigidbodyHandler = new Rigidbody2DHandler(this.TryGetReference<Rigidbody2D>(), Vector2.up);
            var handlerFacade = new Rigidbody2DHandlerFacade(capsule, rigidbodyHandler);

            EntityData = new EntityController2DData<TGroundSensor>(
                capsule,
                _entitySettings,
                stateManager,
                groundSensor,
                handlerFacade
                );

            RegisterAllStates(stateManager);
        }

        #endregion

        #region States
        protected abstract void RegisterAllStates(IStateManager manager);

        protected abstract class EntityState : IState
        {
            protected readonly EntityController2DData<TGroundSensor> EntityData;
            protected readonly Controller2DInputData InputData;

            public EntityState(EntityController2DData<TGroundSensor> controllerData, Controller2DInputData controllerInputData)
            {
                EntityData = controllerData;
                InputData = controllerInputData;
            }

            public virtual void Enter()
            {
            }

            public virtual void Update()
            {
            }

            public virtual void Exit()
            {
            }
        }

        #endregion
    }
}
