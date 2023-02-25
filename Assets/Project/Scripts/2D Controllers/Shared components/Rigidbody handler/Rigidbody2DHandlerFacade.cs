using System.Collections.Generic;
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
            this.Capsule = collider;
            this.Handler = handler;
        }

        #endregion

        #region Logic
        public void DirectedJump(Vector2 normal, float impulse) => Handler.AddImpulseRaw(normal.normalized * impulse);
        public void VerticalJump(float impulse) => Handler.AddVerticalImpulse(impulse);
        public void HorizontalMove(int direction, float force)
        {
            if (direction == 0)
                return;

            Handler.AddHorizontalForce(Mathf.Sign(direction) * force);
        }
        public float CalculateJumpImpulse(float height)
        {
            // jump at height: mass * sqrt(-2 * gravity * jump height)
            // mass ignored
            return Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
        }
        public float CalculateAccelerateForce(float maxSpeed, float distance)
        {
            // moving for time: force = mass * (velocity / time)
            // moving for distance: time = distance / maxSpeed
            // mass ignored
            return maxSpeed / (distance / maxSpeed);
        }
        public float CalculateDecelarateForce(float maxSpeed, float stopDistance)
        {
            if (Handler.HorizontalVelocity == 0)
                return 0;

            // moving for time: force = maxSpeed / decTime
            // moving for distance: force = -velocity / (distance / velocity / 2)
            float force = maxSpeed / (stopDistance / (maxSpeed / 2));
            float direction = -Mathf.Sign(Handler.HorizontalVelocity);

            return Mathf.Min(force, Mathf.Abs(ForceToStop)) * direction;
        }
        public void LimitHorizotalVelocity(float limit)
        {
            Handler.HorizontalVelocity = Mathf.Clamp(Handler.HorizontalVelocity, -limit, limit);
        }

        public bool CanBeSnappedToGround(ContactFilter2D filter, float snapDistance) => CanBeSnappedInternal(filter, snapDistance);
        private RaycastHit2D CanBeSnappedInternal(ContactFilter2D filter, float snapDistance)
        {
            _emptyCastArray[0] = default;
            Physics2D.CapsuleCast(Handler.Position, Capsule.size, Capsule.direction, 0, Vector2.down, filter, _emptyCastArray, snapDistance);

            return _emptyCastArray[0];
        }
        public bool TrySnapToGround(ContactFilter2D filter, float snapDistance)
        {
            RaycastHit2D hitInfo = CanBeSnappedInternal(filter, snapDistance);
            if (hitInfo)
            {
                Snap(Mathf.Max(hitInfo.distance - Physics2D.defaultContactOffset, 0), hitInfo.normal.normalized);
                return true;
            }

            return false;
        }

        private void Snap(float snapDistance, Vector2 normal)
        {
            Body.position -= new Vector2(0, snapDistance);
            Handler.Normal = normal;
        }

        public Vector2 CalculateNormalFromContacts(ContactFilter2D filter)
        {
            int length = Body.GetContacts(filter, _contacts);

            if (length == 0)
                return Vector2.up;

            if (length == 1)
                return _contacts[0].normal.normalized;


            Vector2 sum = new Vector2();
            // using linq is not possible on vectors, thus this ugly loop
            foreach (ContactPoint2D point in _contacts)
                sum += point.normal.normalized;

            return sum / length;
        }

        public bool TrySnapElseUpdateNormal(ContactFilter2D groundFilter, ContactFilter2D snappingFilter, float snapDistance)
        {
            if (TrySnapToGround(snappingFilter, snapDistance))
                return true;

            Handler.Normal = CalculateNormalFromContacts(groundFilter);
            return false;
        }

        #endregion
    }
}
