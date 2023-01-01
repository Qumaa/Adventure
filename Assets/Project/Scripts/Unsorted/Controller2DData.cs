using UnityEngine;
using Project.States;

namespace Project.Controller2D
{
    public class Controller2DData
    {
        private float _maxSpeedCurrent;

        public readonly EntityController2D Controller;
        public readonly CapsuleCollider2D Capsule;

        public int MoveInput { get; set; } = 0;
        public bool JumpInput { get; set; } = false;
        public float CurrentMaxSpeed
        {
            get => _maxSpeedCurrent;
            set => SetMaxSpeed(value);
        }
        public Controller2DSettings Settings { get; private set; }

        public Controller2DData(EntityController2D owner, Controller2DSettings settings, CapsuleCollider2D capsule)
        {
            this.Controller = owner;
            this.Settings = settings;
            this.Capsule = capsule;
        }

        public void SetMaxSpeed(float newSpeed) => _maxSpeedCurrent = Mathf.Max(0, newSpeed);
        public void SetDefaultMaxSpeed() => CurrentMaxSpeed = Settings.MaxSpeed;
    }

}
