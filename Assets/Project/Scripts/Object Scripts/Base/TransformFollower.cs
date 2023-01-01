using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformFollower : GameBehaviour, IFollower
{
    // prefix _ stands for read only values. Make properties with getters or just be careful enough not to change them
    // capitalized first letter stands for values that could be modified / acessed outside
    // non capitalized first letter means that the value belong to this and only this script instance

    [SerializeField] protected Transform _FollowTarget;
    [SerializeField] protected Vector2 _Offset;
    [SerializeField] protected float _SpeedScale = 1;

    protected Vector3 position
    {
        get => transform.position;
        set => transform.position = new Vector3(value.x, value.y, position.z);
    }
    protected Vector3 targetPos => _FollowTarget.position + (Vector3)_Offset;


    private void OnValidate()
    {
        _SpeedScale = Mathf.Max(0, _SpeedScale);
    }

    public virtual void FollowStep(float deltaTime)
    {
        position = Vector2.Lerp(position, targetPos, deltaTime * _SpeedScale);
    }
}


