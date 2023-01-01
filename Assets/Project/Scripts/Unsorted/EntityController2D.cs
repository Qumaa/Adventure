using System;
using UnityEngine;
using Project.States;

namespace Project.Controller2D
{
    public abstract class EntityController2D : GameBehaviour
    {
        #region Values
        // prefix _ stands for read only values. Make properties with getters or just be careful enough not to change them
        // capitalized first letter stands for values that could be modified / acessed outside
        // non capitalized first letter means that the value belong to this and only this script instance

        // Editor values
        [SerializeField] private Controller2DSettings _settings;

        // Internal values
        protected Controller2DData Data { get; private set; }
        protected GroundSensor Sensor { get; private set; }
        protected StateManager StateManager { get; private set; }
        protected Rigidbody2DHandler BodyHandler { get; private set; }
        protected Rigidbody2DHandlerFacade HandlerFacade { get; private set; }

        // Properties
        protected Rigidbody2D Body => BodyHandler.Body;

        #endregion

        #region Mono and inherited callbacks

        protected override void SetUpValues()
        {
            Data = new Controller2DData(this, this._settings, this.TryGetReference<CapsuleCollider2D>())
            {
                MoveInput = 0,
                JumpInput = false
            };
            Data.SetDefaultMaxSpeed();

            Sensor = this.TryGetReference<GroundSensor>();

            BodyHandler = new Rigidbody2DHandler(this.TryGetReference<Rigidbody2D>(), Vector2.up);

            HandlerFacade = new Rigidbody2DHandlerFacade(Data, BodyHandler, Sensor);

            StateManager = new StateManager();
            RegisterAllStates(StateManager);
        }

        private void FixedUpdate()
        {
            this.StateManager.TickStates();

            ConsumeAllInputs();
        }        

        #endregion

        #region Controller logic
        public virtual void Jump() => Data.JumpInput = true;
        protected virtual void ConsumeJumpInput() => Data.JumpInput = false;
        // summing up the input to apply it in fixed update later on
        // not mathf to get 0 if input is 0
        public virtual void Move(int moveDirectionSign) => Data.MoveInput += Math.Sign(moveDirectionSign);
        // float overload
        public void Move(float moveDirectionSign) => Move(Mathf.RoundToInt(moveDirectionSign));
        protected virtual void ConsumeMoveInput() => Data.MoveInput = 0;

        protected virtual void ConsumeAllInputs()
        {
            ConsumeJumpInput();
            ConsumeMoveInput();
        }

        #region States

        protected abstract void RegisterAllStates(StateManager manager);

        #endregion

        #endregion
    }

}

namespace Project.Controller2D
{
    using Project.States;

    public abstract class Grounded : IState
    {
        protected readonly Rigidbody2DHandlerFacade HandlerFacade;
        protected readonly GroundSensor Sensor;
        protected readonly Controller2DData Data;

        public Grounded(Rigidbody2DHandlerFacade facade, GroundSensor sensor, Controller2DData data)
        {
            HandlerFacade = facade;
            Sensor = sensor;
            Data = data;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
            HandlerFacade.ToggleGravity(true);
        }

        public virtual void Update()
        {
            if (Sensor.Ground)
                HandlerFacade.UpdateNormal();
            else 
            if (!HandlerFacade.SnapToFloor())
            {
                OnFailedToSnap();
                return;
            }            

            HandlerFacade.NeglectGravityIfNeeded();

            HandlerFacade.LimitAlignedHorizontalVelocity();
        }

        public virtual void OnFailedToSnap()
        {
        }
    }

    public class Idle : Grounded
    {
        public Idle(Rigidbody2DHandlerFacade facade, GroundSensor sensor, Controller2DData data) : base(facade, sensor, data)
        {
        }

        public override void Update()
        {
            base.Update();

            HandlerFacade.ApplyPausingMotion();
        }
    }
    public class GroundedMove : Grounded
    {
        public GroundedMove(Rigidbody2DHandlerFacade facade, GroundSensor sensor, Controller2DData data) : base(facade, sensor, data)
        {
        }

        public override void Update()
        {
            base.Update();
            HandlerFacade.Move(Data.MoveInput);
        }
    }

    public abstract class Midair : IState
    {
        protected readonly Rigidbody2DHandlerFacade HandlerFacade;
        protected readonly Controller2DData Data;

        public Midair(Rigidbody2DHandlerFacade handler, Controller2DData data)
        {
            HandlerFacade = handler;
            Data = data;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }
    }

    public class Jumping : Midair
    {
        public Jumping(Rigidbody2DHandlerFacade handler, Controller2DData data) : base(handler, data)
        {
        }

        public override void Enter()
        {
            HandlerFacade.AlignedJump((Vector2.up + HandlerFacade.Handler.AlignedNormal) / 2);
        }
    }
    public class ControlledMidair : Midair
    {
        protected readonly float Limit;

        public ControlledMidair(Rigidbody2DHandlerFacade handler, Controller2DData data, float fallSpeedLimit) : base(handler, data)
        {
            Limit = fallSpeedLimit;
        }

        public override void Update()
        {
            HandlerFacade.LimitAlignedVelocity(Vector2.up, Limit);
        }
    }
}
