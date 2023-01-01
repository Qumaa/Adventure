using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Controller2D
{
    public sealed class GroundSensor : GameBehaviour, IVisualisible
    {
        #region Values
        // prefix _ stands for read only values. Make properties with getters or just be careful enough not to change them
        // capitalized first letter stands for values that could be modified / acessed outside
        // non capitalized first letter means that the value belong to this and only this script instance

        #region Editor values
        [SerializeField] private FiltersHolderAutomated _FilterHolder;
        #endregion

        #region Internal values

        #endregion

        #region References
        private Rigidbody2D itsBody;
        private Collider2D itsBox;
        #endregion

        #region Properties
        public FiltersHolderAutomated Filters => _FilterHolder;
        public bool Ground => itsBody.IsTouching(Filters.Ground);
        public bool Slope => itsBody.IsTouching(Filters.Slope);
        public bool Ceiling => itsBody.IsTouching(Filters.Ceiling);
        public bool Right => itsBody.IsTouching(Filters.Right);
        public bool Left => itsBody.IsTouching(Filters.Left);
        public bool Anything => itsBody.IsTouching(Filters.Anything);
        #endregion

        #endregion

        #region Logic

        #region Internal

        private void FixedUpdate()
        {
            Visualise();
        }

        #endregion

        #region GroundSensor

        public GroundData GetData() => GroundData.FromFilters(itsBody, _FilterHolder);

        protected override void SetUpValues()
        {
            this.TryGetReference(ref itsBox);
            this.TryGetReference(ref itsBody);
            this.TryGetReference(ref _FilterHolder);
        }

        #endregion

        #region Utility

        public override string ToString() => $"{base.ToString()}: Grounded-{Ground}; Walls-{GetWalls()}";
        private string GetWalls()
        {
            if (Right && Left) return "Both";
            if (Right) return "Right";
            if (Left) return "Left";

            return "None";
        }

        public void Visualise()
        {
            Vector2 rightSize = new Vector2(itsBox.bounds.extents.x, 0);
            Vector2 upSize = new Vector2(0, itsBox.bounds.extents.y);
            Vector2 pos = itsBox.bounds.center;

            DrawRay(pos, rightSize, Right); // right
            DrawRay(pos, -rightSize, Left); // left

            DrawRay(pos, upSize, Ceiling); // up
            DrawRay(pos, -upSize, Ground); // down
        }

        #endregion

        #endregion
    }
}
