using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;
using Project.States;

public class TempController : EntityController2D
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.Sensor.Ground)
        {
            this.Jump();
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            base.Move(Input.GetAxisRaw("Horizontal"));
        }
    }

    protected override void RegisterAllStates(StateManager manager)
    {
        throw new System.NotImplementedException();
    }

    #region States

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

    public class GroundedIdle : Grounded
    {
        public GroundedIdle(Rigidbody2DHandlerFacade facade, GroundSensor sensor, Controller2DData data) : base(facade, sensor, data)
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

    #endregion
}
