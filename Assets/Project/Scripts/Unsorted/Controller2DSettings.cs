using UnityEngine;

namespace Project.Controller2D
{
    [CreateAssetMenu(fileName = "New Controller2D Settings", menuName = ProjectData.AssetMenuPaths.Controller2D + "/Controller2D Settings")]
    public class Controller2DSettings : ScriptableObject
    {
        [SerializeField] private float _jumpHeight;             // +
        [SerializeField] private float _maxSpeed;               // +
        [SerializeField] private float _accelrationDistance;    // +
        [SerializeField] private float _decelerationDistance;   // +
        [SerializeField] private float _snapDistance;           // +

        public virtual float JumpHeight => _jumpHeight;
        public virtual float MaxSpeed => _maxSpeed;
        public virtual float AccelerationDistance => _accelrationDistance;
        public virtual float DecelerationDistance => _decelerationDistance;
        public virtual float SnapDistance => _snapDistance;
    }
}
