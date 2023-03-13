using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;
using Project.States;

public class TemporalCharacterController : PlayerController2D
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
            _inputData.ClearJumpInput();
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            this.Move(Input.GetAxisRaw("Horizontal"));
        }
        else this._inputData.ClearMoveInput();
    }

    protected override void RegisterAllStates(IStateManager manager)
    {
        var state = new GodlikeGroundState(_inputData, _entityData, _playerData);
        manager.AssignDefaultState(state);
    }

    #endregion

    #region TemporalCharacterController logic



    #endregion

    #region Math, shortcuts and utility



    #endregion

    // interfaces implementations
}
