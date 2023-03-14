using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Controller2D
{
    public class Rigidbody2DHandlerFacade
    {
        #region Values

        private readonly RaycastHit2D[] _emptyCastArray = new RaycastHit2D[1];
        private readonly List<ContactPoint2D> _contacts = new List<ContactPoint2D>(5);

        // properties
        protected Rigidbody2D Body => Handler.Body;
        protected CapsuleCollider2D Capsule { get; private set; }
        public Rigidbody2DHandler Handler { get; private set; }
        protected float ForceToStop => -Handler.HorizontalVelocity / Time.fixedDeltaTime;

        // constructor
        public Rigidbody2DHandlerFacade(CapsuleCollider2D collider, Rigidbody2DHandler handler)
        {
            Capsule = collider;
            Handler = handler;
        }

        #endregion

        #region Logic
        // facade operations
        public void Jump(float height) => 
            Handler.AddVerticalImpulse(CalculateJumpImpulse(height));
        public void Move(int direction, float maxSpeed, float accelerationDistance)
        {
            if (direction == 0)
                return;

            Handler.AddHorizontalForce(Mathf.Sign(direction) * CalculateAccelerateForce(maxSpeed, accelerationDistance));
            LimitHorizotalVelocity(maxSpeed);
        }
        public void Brake(float maxSpeed, float brakingDistance)
        {
            float force = CalculateDecelarateForce(maxSpeed, brakingDistance);
            
            if (force == 0) return;
            
            Handler.AddHorizontalForce(force);
        }
        public void LimitHorizotalVelocity(float limit) => 
            Handler.HorizontalVelocity = Mathf.Clamp(Handler.HorizontalVelocity, -limit, limit);

        public bool CanBeSnapped(ContactFilter2D filter, float snapDistance) => 
            CanBeSnappedInternal(filter, snapDistance);
        public void SnapAndUpdateNormal(ContactFilter2D normalFilter, ContactFilter2D snappingFilter,
            float snapDistance)
        {
            RaycastHit2D hitInfoSnap = CanBeSnappedInternal(snappingFilter, snapDistance);
            
            if (!hitInfoSnap) return;

            RaycastHit2D hitInfoNormal = normalFilter.Equals(snappingFilter) ? hitInfoSnap : CanBeSnappedInternal(normalFilter, snapDistance);
            Vector2 normal = hitInfoNormal ? hitInfoSnap.normal.normalized : Vector2.up;
            if (Handler.Normal == normal) return;

            float snap = Mathf.Max(hitInfoSnap.distance - Physics2D.defaultContactOffset, 0);
            SnapInternal(snap, normal);
        }

        public void UpdateNormal(ContactFilter2D groundFilter) =>
            UpdateNormal(CalculateNormalFromContacts(groundFilter));
        public void UpdateNormal(Vector2 normal) =>
            Handler.Normal = normal.normalized;
        
        public void ApplyInputFrame()
        {
            Handler.UpdateWorldVelocity();
        }
        
        public void AccumulateGravity() => 
            Handler.AccumulateGravity();
        public void AccumulateGravity(float gravity) =>
            Handler.AccumulateGravity(gravity);

        public void ResetAccumulatedGravity() => 
            Handler.VerticalVelocity = 0;

        // force calculations
        private float CalculateJumpImpulse(float height)
        {
            // jump at height: mass * sqrt(-2 * gravity * jump height)
            // mass ignored
            return Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
        }
        private float CalculateAccelerateForce(float maxSpeed, float accelarationDistance)
        {
            // moving for time: force = mass * (velocity / time)
            // moving for distance: time = distance / maxSpeed
            // mass ignored
            return maxSpeed / (accelarationDistance / maxSpeed);
        }
        private float CalculateDecelarateForce(float speed, float stopDistance)
        {
            if (Handler.HorizontalVelocity == 0)
                return 0;

            // moving for time: force = maxSpeed / decTime
            // moving for distance: force = -velocity / (distance / velocity / 2)
            float force = speed / (stopDistance / (speed / 2));
            float direction = -Mathf.Sign(Handler.HorizontalVelocity);

            return Mathf.Min(force, Mathf.Abs(ForceToStop)) * direction;
        }

        // snapping
        private RaycastHit2D CanBeSnappedInternal(ContactFilter2D filter, float snapDistance)
        {
            _emptyCastArray[0] = default;
            Physics2D.CapsuleCast(Handler.Position, Capsule.size, Capsule.direction, 0, Vector2.down, filter, _emptyCastArray, snapDistance);

            return _emptyCastArray[0];
        }
        private void SnapInternal(float snapDistance, Vector2 normal)
        {
            Body.position -= new Vector2(0, snapDistance);
            UpdateNormal(normal);
        }
        
        // normal related
        private Vector2 CalculateNormalFromContacts(ContactFilter2D filter)
        {
            int length = Body.GetContacts(filter, _contacts);

            switch (length)
            {
                case 0:
                    return Vector2.up;
                case 1:
                    return _contacts[0].normal.normalized;
            }

            Vector2 sum = new Vector2();
            sum = _contacts.Aggregate(sum, (current, point) => current + point.normal.normalized);

            return sum / length;
        }

        #endregion

        
    }
}
