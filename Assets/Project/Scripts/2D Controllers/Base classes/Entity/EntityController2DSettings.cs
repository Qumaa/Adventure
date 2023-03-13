using UnityEngine;

namespace Project.Controller2D
{
    [CreateAssetMenu(fileName = "New Entity Controller Settings", menuName = ProjectData.AssetMenuPaths.Controller2D + "/Entity Controller Settings")]
    public sealed class EntityController2DSettings : ScriptableObject
    {
        [SerializeField] private float _jumpHeight;             // +
        [SerializeField] private float _maxSpeed;               // +
        [SerializeField] private float _accelrationDistance;    // +
        [SerializeField] private float _decelerationDistance;   // +
        [SerializeField] private float _snapDistance;           // +

        public float JumpHeight => _jumpHeight;
        public float MaxSpeed => _maxSpeed;
        public float AccelerationDistance => _accelrationDistance;
        public float DecelerationDistance => _decelerationDistance;
        public float SnapDistance => _snapDistance;
    }
}
