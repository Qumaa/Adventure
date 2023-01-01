using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Controller2D
{
    public class Rigidbody2DHandler
    {
        #region Values

        private Vector2 _cachedNormal;
        private Rigidbody2D itsBody;

        #endregion

        #region Properties

        public Rigidbody2D Body => itsBody;

        public Vector2 Position
        {
            get => Body.position;
            set => Body.position = value;
        }
        public Vector2 Velocity
        {
            get => Body.velocity;
            set => Body.velocity = value;
        }

        public Vector2 AlignedNormal
        {
            get => _cachedNormal;
            set => SetAlignedNormal(value);
        }

        public Vector2 AlignedRight => GetAlignedRight(AlignedNormal);
        public Vector2 AlignedLeft => GetAlignedLeft(AlignedNormal);
        public float AlignedHorizontalVelocity => GetAlignedVelocity(AlignedRight);
        public float AlignedVerticalVelocity => GetAlignedVelocity(AlignedNormal);

        public float ForceToStop => -AlignedHorizontalVelocity / Time.fixedDeltaTime;

        #endregion

        #region Constructors

        public Rigidbody2DHandler(Rigidbody2D body, Vector2 normal)
        {
            itsBody = body;
            SetAlignedNormal(normal);
        }

        public Rigidbody2DHandler(Rigidbody2D body)
        {
            itsBody = body;
            SetAlignedNormal(Vector2.up);
        }

        #endregion

        #region Logic
        // basic operations
        private void AddForce(Vector2 force, ForceMode2D mode) => Body.AddForce(force, mode);
        public void AddForce(Vector2 force) => AddForce(force, ForceMode2D.Force);
        public void AddImpulse(Vector2 impulse) => AddForce(impulse, ForceMode2D.Impulse);

        // add force prallel to the ground
        public void AddAlignedForce(float force, Vector2 aligned, ForceMode2D mode = ForceMode2D.Force)
        {
            if (force == 0) return;

            AddForce(aligned * force, mode);
        }
        public void AddAlignedForce(float force, ForceMode2D mode) => AddAlignedForce(force, AlignedRight, mode);
        public void AddAlignedForce(float force) => AddAlignedForce(force, ForceMode2D.Force);

        // add impulse parallel to the ground
        public void AddAlignedImpulse(float impulse) => AddAlignedForce(impulse, ForceMode2D.Impulse);
        public void AddAlignedImpulse(float impulse, Vector2 aligned) => AddAlignedForce(impulse, aligned, ForceMode2D.Impulse);

        // aligned values
        public Vector2 GetAlignedRight(Vector2 normal) => new Vector2(normal.y, -normal.x);
        public Vector2 GetAlignedLeft(Vector2 normal) => -GetAlignedRight(normal);
        public float GetAlignedVelocity(Vector2 normal) => Vector2.Dot(normal, Velocity);
        private void SetAlignedNormal(Vector2 newNormal)
        {
            if (newNormal == Vector2.zero) throw new Exception("Rigidbody Handler got invalid normal vector: normal cannot be zero!");

            this._cachedNormal = newNormal.normalized;
        }

        // normal related
        public Vector2 CalculateAlignedNormal(ContactFilter2D floorFilter)
        {
            List<ContactPoint2D> contacts = new List<ContactPoint2D>();
            int length = Body.GetContacts(floorFilter, contacts);

            if (length == 0) return Vector2.up;

            // select normal values
            Vector2 sum = new Vector2();
            foreach (ContactPoint2D point in contacts)
                sum += point.normal.normalized;

            return sum / length;
        }
        public Vector2 CalculateAlignedNormal(GroundSensor sensor) => CalculateAlignedNormal(sensor.Filters.Ground);
        public void CacheGroundNormal(Vector2 newNormal) => AlignedNormal = newNormal;

        #endregion
    }

}
