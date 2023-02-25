using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformFollower : MonoBehaviour, IFollower
{
    // prefix _ stands for read only values. Make properties with getters or just be careful enough not to change them
    // capitalized first letter stands for values that could be modified / acessed outside
    // non capitalized first letter means that the value belong to this and only this script instance

    [SerializeField] protected Transform _followTarget;
    [SerializeField] protected Vector2 _offset;
    [SerializeField] protected float _speedScale = 1;

    protected Vector3 _position
    {
        get => transform.position;
        set => transform.position = new Vector3(value.x, value.y, _position.z);
    }
    protected Vector3 _targetPos => _followTarget.position + (Vector3)_offset;


    private void OnValidate()
    {
        _speedScale = Mathf.Max(0, _speedScale);
    }

    public virtual void FollowStep(float deltaTime)
    {
        _position = Vector2.Lerp(_position, _targetPos, deltaTime * _speedScale);
    }
}


