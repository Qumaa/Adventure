using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;
using Project.States;

public class TemporalCharacterController : PlayerController2D<IGroundSensorPlayer>
{
    #region Values
    // public, protected: Name
    // private: _name
    // small scope: name
    // references / components: itsName

    // Editor values


    // Internal values


    // References


    // Properties

    #endregion

    #region Mono and inherited methods

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // get references


        // set up values

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            InputData.ClearJumpInput();
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            this.Move(Input.GetAxisRaw("Horizontal"));
        }
        else this.InputData.ClearMoveInput();
    }

    protected override bool FallingCondition()
    {
        return !(EntityData.Sensor.Ground || EntityData.Sensor.Slope);
    }

    protected override void UpdateNormal()
    {
        if (!EntityData.Sensor.Ground)
        {
            EntityData.HandlerFacade.Handler.Normal = Vector2.up;
            return;
        }

        EntityData.HandlerFacade.CalculateNormalFromContacts(EntityData.Sensor.Filters.Ground);
    }

    protected override void RegisterAllStates(IStateManager manager)
    {
        
    }

    #endregion

    #region TemporalCharacterController logic



    #endregion

    #region Math, shortcuts and utility



    #endregion

    // interfaces implementations
}
