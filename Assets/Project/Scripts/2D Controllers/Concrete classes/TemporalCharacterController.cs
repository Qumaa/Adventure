using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D.Player;
using Project.States;
using UnityEditor;

// TODO: a midair-grounded transition fix:
// if landed off and can be snapped, snap and stay in grounded
// else switch to air states

public class TemporalCharacterController : PlayerController2D
{
    #region Values

    #endregion

    #region Mono and inherited methods

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

    private void OnGUI()
    {
        if (_entityData.StateManager.CurrentState is PlayerSubStateManager states) 
            Handles.Label(transform.position, states.CurrentState.ToString());
    }

    protected override void RegisterAllStates(IStateManager manager)
    {
        var sensor = _entityData.Sensor;
        
        // sub managers
        var groundedStates = new PlayerGroundedSubStateManager(_entityData, _playerData);
        var airStates = new PlayerMidairSubStateManager(_entityData, _playerData);
        
        // main
        manager.RegisterDefaultState(groundedStates)
            .RegisterState(airStates);

        manager.AddBidirectionalTransition(groundedStates, airStates,
            () => !_entityData.Sensor.GroundOrSlope);
        
        // ground
        var idleGroundedState = new IdleGroundedPlayerState(_inputData, _entityData, _playerData);
        var movingGroundedState = new MovingGroundedPlayerState(_inputData, _entityData, _playerData);
        var slidingState = new SlidingPlayerState(_inputData, _entityData, _playerData);
        
        groundedStates.RegisterDefaultState(idleGroundedState)
            .RegisterState(movingGroundedState)
            .RegisterState(slidingState);

        groundedStates.AddAnyTransition(slidingState, () => sensor.GroundOrSlope && !sensor.Ground)
            .AddTransition(slidingState, idleGroundedState, () => sensor.Ground)
            .AddBidirectionalTransition(idleGroundedState, movingGroundedState, () => _inputData.Movement != 0);
        
        // air
        var idleAirState = new IdleMidairPlayerState(_inputData, _entityData, _playerData);
        var movingAirState = new MovingMidairPlayerState(_inputData, _entityData, _playerData);

        airStates.RegisterDefaultState(idleAirState)
            .RegisterState(movingAirState);

        airStates.AddBidirectionalTransition(idleAirState, movingAirState, () => _inputData.Movement != 0);
    }

    #endregion
}
