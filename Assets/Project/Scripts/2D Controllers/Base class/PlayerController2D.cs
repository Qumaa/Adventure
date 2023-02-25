using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;
using Project.States;

public abstract class PlayerController2D<TGroundSensor> : EntityController2D<TGroundSensor> where TGroundSensor : IGroundSensorPlayer
{
    #region Values

    [SerializeField] private PlayerController2DSettings _playerSettings;

    protected PlayerController2DData PlayerData { get; private set; }

    #endregion

    #region Mono and inherited methods

    protected override void Awake()
    {
        base.Awake();
        PlayerData = new PlayerController2DData(_playerSettings);
    }

    #endregion

    #region Controller logic

    public override void Jump()
    {
        InputData.GiveJumpInput();
    }

    public override void Move(int moveDirectionSign)
    {
        InputData.GiveMoveInput(moveDirectionSign);
    }

    #endregion
}

public interface ICountable
{
    public bool Started { get; }
    public bool InProgress { get; }
    public bool Finished { get; }
    public void Tick(float deltaTime);
}

public class StaticCounter : ICountable
{
    private float _duration;
    private float _elapsed;

    // 0 = idle, 1 = in progress, 2 = finished, 3 = break
    private byte _state;

    public event System.Action OnStarted, OnFinished, OnBreak;

    public StaticCounter(float duration)
    {
        _duration = duration;
        _elapsed = 0;
        _state = 0;
    }

    // methods
    public void Start()
    {
        _elapsed = 0;

        _state = 1;

        OnStarted?.Invoke();

        if (_duration == 0) Finish();
    }
    private void Finish()
    {
        _state = 2;

        OnFinished?.Invoke();
    }
    public bool Break()
    {
        if (!InProgress) return false;

        _state = 3;

        OnBreak?.Invoke();
        OnFinished?.Invoke();

        return true;
    }

    public void SetDuration(float newDuration)
    {
        if (!InProgress) _duration = newDuration;
    }

    // interface
    public bool Started => _state > 0;
    public bool InProgress => _state == 1;
    public bool Finished => _state > 1;
    public float Progress => _elapsed / _duration;
    public float Duration => _duration;


    public void Tick(float deltaTime)
    {
        if (InProgress)
        {
            if (_elapsed >= _duration)
                Finish();
            else
                _elapsed += deltaTime;
        }
    }
}
