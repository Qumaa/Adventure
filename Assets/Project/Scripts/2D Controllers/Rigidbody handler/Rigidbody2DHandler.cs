using System;
using UnityEngine;

namespace Project.Controller2D
{
    public class Rigidbody2DHandler
    {
        #region Values

        private Vector2 _cachedNormal;
        private readonly Rigidbody2D _itsBody;

        #endregion

        #region Properties

        public Rigidbody2D Body => _itsBody;
        public Vector2 Position
        {
            get => Body.position;
            set => Body.position = value;
        }
        public Vector2 WorldVelocity
        {
            get => Body.velocity;
            set => Body.velocity = value;
        }
        public Vector2 LocalVelocity
        {
            get => GetLocalVelocity();
            set => SetLocalVelocity(value);
        }
        public float GravityScale { get; set; }

        public Vector2 Normal
        {
            get => _cachedNormal;
            set => SetNormal(value);
        }
        public Vector2 NormalRight => GetNormalRight(Normal);
        public Vector2 NormalLeft => GetNormalLeft(Normal);

        public float VerticalVelocity { get; set; }
        public float HorizontalVelocity { get; set; }
        public Vector2 CalculatedVelocity => CalculateWorldVelocity();

        #endregion

        #region Constructors

        public Rigidbody2DHandler(Rigidbody2D body, Vector2 normal)
        {
            _itsBody = body;
            GravityScale = _itsBody.gravityScale;
            _itsBody.gravityScale = 0;

            SetNormal(normal);
        }
        public Rigidbody2DHandler(Rigidbody2D body) : this(body, Vector2.up)
        {
        }

        #endregion

        #region Logic
        // basic operations parallel to the ground
        private void AddHorizontalForce(float force, ForceMode2D mode)
        {
            if (force == 0)
                return;

            HorizontalVelocity += EmulateForceAddition(force, mode);
        }
        public void AddHorizontalForce(float force) => AddHorizontalForce(force, ForceMode2D.Force);
        public void AddHorizontalImpulse(float impulse) => AddHorizontalForce(impulse, ForceMode2D.Impulse);

        // basic operations for vertical velocity
        private void AddVerticalForce(float force, ForceMode2D mode)
        {
            if (force == 0)
                return;

            VerticalVelocity += EmulateForceAddition(force, mode);
        }
        public void AddVerticalForce(float force) => AddVerticalForce(force, ForceMode2D.Force);
        public void AddVerticalImpulse(float impulse) => AddVerticalForce(impulse, ForceMode2D.Impulse);

        // aligned values
        private Vector2 GetNormalRight(Vector2 normal)
        {
            normal.Normalize();
            return new Vector2(normal.y, -normal.x);
        }
        private Vector2 GetNormalLeft(Vector2 normal) => -GetNormalRight(normal);
        private void SetNormal(Vector2 newNormal)
        {
            if (newNormal == Vector2.zero)
                throw new Exception("Rigidbody Handler got invalid normal vector: normal cannot be zero!");

            _cachedNormal = newNormal.normalized;
        }

        // vertical velocity from gravity
        public void AccumulateVerticalVelocity(float velocity) => VerticalVelocity += velocity;
        public void AccumulateVerticalVelocity() =>
            AccumulateVerticalVelocity(OneStepPhysicsValue);

        // velocity interactions
        public void UpdateWorldVelocity() => WorldVelocity = CalculateWorldVelocity();
        private Vector2 CalculateWorldVelocity() => (NormalRight * HorizontalVelocity) + new Vector2(0, VerticalVelocity);
        private Vector2 GetLocalVelocity() => new Vector2(HorizontalVelocity, VerticalVelocity);

        private void SetLocalVelocity(Vector2 velocity)
        {
            HorizontalVelocity = velocity.x;
            VerticalVelocity = velocity.y;
        }

        // extracted methods
        private float EmulateForceAddition(float rawForce, ForceMode2D mode) =>
            mode == ForceMode2D.Impulse ? rawForce : rawForce * Time.fixedDeltaTime;

        private float OneStepPhysicsValue => Physics2D.gravity.y / Body.mass * GravityScale * Time.fixedDeltaTime;

        #endregion
    }
}
