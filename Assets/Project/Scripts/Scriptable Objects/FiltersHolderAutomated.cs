using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;

namespace Project.Controller2D
{
    [CreateAssetMenu(fileName = "New Filters Holder", menuName = ProjectData.AssetMenuPaths.Controller2D + "/Filters Holder")]
    public sealed class FiltersHolderAutomated : FiltersHolderBase
    {
        // editor
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] [Range(0, 85)] private float _maxFloorAngle;
        [SerializeField] [Range(0, 5)] private float _wallError;

        private ContactFilter2D _slope = new ContactFilter2D();

        public ContactFilter2D Slope => _slope;
        public override LayerMask GroundLayer => _groundLayers;        

        // angles
        private const float up = 270, down = 90, right = 180, left = 0;

        private void OnValidate()
        {
            // TODO: gui thing like in navmesh agent

            SetFilters();
        }

        private void SetFilters()
        {
            // TODO
            SetAllFiltersMask(_groundLayers);
            SetAllFiltersAngles();
        }

        private void SetAllFiltersAngles()
        {
            SetFilterAngles(ref _Ground, down, _maxFloorAngle);
            SetFilterAngles(ref _Ceiling, up, _maxFloorAngle);

            SetFilterAngles(ref _Right, right, _wallError);
            SetFilterAngles(ref _Left, left, _wallError);

            SetFilterAngles(ref _slope, down, 90 - _wallError / 2);
        }

        protected override void SetAllFiltersMask(LayerMask mask)
        {
            base.SetAllFiltersMask(mask);
            SetFilterMask(ref _slope, mask);
        }
    }
}
