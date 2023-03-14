using Project.States;
using UnityEngine;

namespace Project.Controller2D
{
    public abstract class EntityController2D<TGroundSensor> : MonoBehaviour where TGroundSensor : IGroundSensor
    {
        #region Values

        // Editor values
        [SerializeField] private EntityController2DSettings _entitySettings;

        // Internal values
        protected EntityController2DData<TGroundSensor> _entityData { get; private set; }
        protected Controller2DInputData _inputData { get; private set; }
        #endregion

        #region Controller logic


        private void Awake() => CreateDataInternal();
        protected virtual void FixedUpdate()
        {
            _entityData.StateManager.TickStates();
        }
        
        /// <summary>
        /// This method doesn't stand for jumping, but rather for processing and caching the input
        /// </summary>
        public virtual void Jump() => _inputData.GiveJumpInput();
        /// <summary>
        /// This method doesn't stand for moving, but rather for processing and caching the input
        /// </summary>
        public virtual void Move(int moveDirectionSign) => _inputData.GiveMoveInput(moveDirectionSign);
        // float overload
        public void Move(float moveDirectionSign) => Move(Mathf.RoundToInt(moveDirectionSign));

        private void CreateDataInternal()
        {
            if (_entityData != null)
                return;

            _inputData = new Controller2DInputData();

            var capsule = this.TryGetReference<CapsuleCollider2D>();
            var stateManager = new StateManager();
            var groundSensor = this.TryGetReference<TGroundSensor>();
            var rigidbodyHandler = new Rigidbody2DHandler(this.TryGetReference<Rigidbody2D>(), Vector2.up);
            var handlerFacade = new Rigidbody2DHandlerFacade(capsule, rigidbodyHandler);

            _entityData = new EntityController2DData<TGroundSensor>(
                capsule,
                _entitySettings,
                stateManager,
                groundSensor,
                handlerFacade
                );

            CreateData();
            
            RegisterAllStates(stateManager);
        }

        protected virtual void CreateData() { }

        #endregion

        #region States
        protected abstract void RegisterAllStates(IStateManager manager);

        #endregion
    }
}


