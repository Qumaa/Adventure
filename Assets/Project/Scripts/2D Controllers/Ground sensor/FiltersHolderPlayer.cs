using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Controller2D;

namespace Project.Controller2D
{
    [CreateAssetMenu(fileName = "New Filters Holder", menuName = ProjectData.AssetMenuPaths.Controller2D + "/Filters Holder")]
    public class FiltersHolderPlayer : FiltersHolderBase
    {
        // editor
        private const float _MAX_WALL_ERROR = 5;
        private const float _MAX_FLOOR_ANGLE = 90 - _MAX_WALL_ERROR / 2f;
        
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] [Range(0, _MAX_FLOOR_ANGLE)] private float _maxFloorAngle;
        [SerializeField] [Range(0, _MAX_WALL_ERROR)] private float _wallError;

        private ContactFilter2D _groundOrSlope = new ContactFilter2D();

        public ContactFilter2D GroundOrSlope => _groundOrSlope;
        public override LayerMask GroundLayer => _groundLayers;        

        // angles
        private const float up = 270, down = 90, right = 180, left = 0;

        private void OnValidate()
        {
            // TODO: gui thing like in navmesh agent

            UpdateFilters();
        }

        private void UpdateFilters()
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

            SetFilterAngles(ref _groundOrSlope, down, _MAX_FLOOR_ANGLE);
        }

        protected override void SetAllFiltersMask(LayerMask mask)
        {
            base.SetAllFiltersMask(mask);
            SetFilterMask(ref _groundOrSlope, mask);
        }
    }
}
