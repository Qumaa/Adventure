using UnityEngine;

[CreateAssetMenu(fileName = "New Player Controller Settings", menuName = ProjectData.AssetMenuPaths.Controller2D + "/Player Controller Settings")]
public class PlayerController2DSettings : ScriptableObject
{
    [SerializeField] private float _jumpMaxDuration;
    [SerializeField] private float _jumpBufferingTime;
    [SerializeField] private float _fallGravity;
    [SerializeField] private float _fallVelocityLimit;
    [SerializeField] private float _collisionForgivingTime;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private float _apexTime;
    [SerializeField] private float _apexSpeedModifier;

    private float _fallGravityScale;

    public float FallGravityScale => _fallGravityScale;
    public float JumpMaxDuration => _jumpMaxDuration;
    public float JumpBufferingTime => _jumpBufferingTime;
    public float FallVelocityLimit => _fallVelocityLimit;
    public float CollisionForgivingTime => _collisionForgivingTime;
    public float CoyoteTime => _coyoteTime;
    public float ApexTime => _apexTime;
    public float ApexSpeedModifier => _apexSpeedModifier;    

    private void OnValidate()
    {
        _jumpBufferingTime = Mathf.Max(0, _jumpBufferingTime);

        _fallGravity = Mathf.Max(0, _fallGravity);
        _fallGravityScale = Mathf.Abs(_fallGravity / Physics2D.gravity.y);
        _fallVelocityLimit = Mathf.Max(0, _fallVelocityLimit);

        _collisionForgivingTime = Mathf.Max(0, _collisionForgivingTime);

        _coyoteTime = Mathf.Max(0, _coyoteTime);

        _apexTime = Mathf.Max(0, _apexTime);
        _apexSpeedModifier = Mathf.Max(0, _apexSpeedModifier);
    }
}
