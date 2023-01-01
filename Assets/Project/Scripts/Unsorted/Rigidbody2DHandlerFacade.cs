using UnityEngine;

namespace Project.Controller2D
{
    // todo
    public class Rigidbody2DHandlerFacade
    {
        #region Values

        protected Rigidbody2D Body => Handler.Body;
        protected Controller2DData Data { get; private set; }
        public Rigidbody2DHandler Handler { get; private set; }
        protected GroundSensor Sensor { get; private set; }
        public virtual float JumpImpulse => CalculateJumpImpulse(Data.Settings.JumpHeight);
        public virtual float AccelerateForce => CalculateAccelerateForce(Data.CurrentMaxSpeed, Data.Settings.AccelerationDistance);
        public virtual float DecelerateForce => CalculateDecelarateForce(Data.CurrentMaxSpeed, Data.Settings.DecelerationDistance);

        public Rigidbody2DHandlerFacade(Controller2DData data, Rigidbody2DHandler handler, GroundSensor sensor)
        {
            this.Data = data;
            this.Handler = handler;
            this.Sensor = sensor;
        }

        #endregion

        #region Logic
        public void VerticalJump() => Handler.AddImpulse(new Vector2(0, JumpImpulse));
        public void AlignedJump(Vector2 normal) => Handler.AddImpulse(normal.normalized * JumpImpulse);
        public void Move(int direction)
        {
            if (direction == 0) return;
            Handler.AddAlignedForce(Mathf.Sign(direction) * AccelerateForce);
        }

        public float CalculateJumpImpulse(float height)
        {
            // jump at height: mass * sqrt(-2 * gravity * jump height)
            float imp = Body.mass * Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
            // velocity correction to maintain persistent height
            imp -= Mathf.Clamp(Handler.Velocity.y, 0, imp);
            return imp;
        }
        public float CalculateAccelerateForce(float maxSpeed, float distance)
        {
            // moving for time: force = mass * (velocity / time)
            // moving for distance: time = distance / maxSpeed
            return Body.mass * (maxSpeed / (distance / maxSpeed));
        }
        public float CalculateDecelarateForce(float maxSpeed, float stopDistance)
        {
            if (Handler.AlignedHorizontalVelocity == 0) return 0;

            // moving for time: force = maxSpeed / decTime
            // moving for distance: force = -velocity / (distance / velocity / 2)
            return Mathf.Min(maxSpeed / (stopDistance / (maxSpeed / 2)), Mathf.Abs(Handler.ForceToStop)) * -Mathf.Sign(Handler.AlignedHorizontalVelocity);
        }

        public void ApplyPausingMotion() => Handler.AddAlignedForce(DecelerateForce);
        public void LimitAlignedHorizontalVelocity() => LimitAlignedVelocity(Handler.AlignedRight, Data.CurrentMaxSpeed);
        public void LimitAlignedVelocity(Vector2 axis, float limit)
        {
            // if velocity along the ground is smaller than limit, quit 
            axis = axis.normalized;
            float velocity = Handler.GetAlignedVelocity(axis);
            float velocityAbs = Mathf.Abs(velocity);
            if (limit >= velocityAbs) return;

            // impulse of inversed difference between aligned velocity and limit
            Handler.AddImpulse((velocityAbs - limit) * Mathf.Sign(velocity) * -axis);
        }
        public void NeglectGravityIfNeeded()
        {
            if (Handler.AlignedNormal != Vector2.up) ToggleGravity(false);
            else ToggleGravity(true);
        }

        public void ToggleGravity(bool active)
        {
            Body.gravityScale = active ? 1 : 0;
        }
        public RaycastHit2D CanBeSnappedToFloor() =>
            Physics2D.CapsuleCast(Handler.Position, Data.Capsule.size, Data.Capsule.direction, 0, Vector2.down, Data.Settings.SnapDistance, Sensor.Filters.GroundLayer);
        public bool SnapToFloor()
        {
            RaycastHit2D hitInfo;
            if (hitInfo = CanBeSnappedToFloor())
            {
                float xVel = Handler.AlignedHorizontalVelocity; // get horizontal velocity
                Handler.AddImpulse(-Handler.Velocity); // stop rigidbody
                Handler.Position -= new Vector2(0, hitInfo.distance); // reposition it

                Handler.CacheGroundNormal(hitInfo.normal.normalized); // force new normal value
                Handler.AddAlignedImpulse(xVel); // set horizontal velocity back
                return true;
            }

            return false;
        }
        public void UpdateNormal() => Handler.CacheGroundNormal(Handler.CalculateAlignedNormal(Sensor.Filters.Ground));

        #endregion
    }

}
