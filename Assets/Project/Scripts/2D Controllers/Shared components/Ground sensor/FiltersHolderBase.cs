using UnityEngine;

namespace Project.Controller2D
{
    public abstract class FiltersHolderBase : ScriptableObject
    {

        // internal
        protected ContactFilter2D _Ground;
        protected ContactFilter2D _Left;
        protected ContactFilter2D _Right;
        protected ContactFilter2D _Ceiling;
        protected ContactFilter2D _Layer;

        // props
        public virtual ContactFilter2D Ground => _Ground;
        public virtual ContactFilter2D Left => _Left;
        public virtual ContactFilter2D Right => _Right;
        public virtual ContactFilter2D Ceiling => _Ceiling;
        public virtual ContactFilter2D Anything => _Layer;
        public abstract LayerMask GroundLayer { get; }

        // methods
        protected virtual void SetAllFiltersMask(LayerMask mask)
        {
            SetFilterMask(ref _Right, mask);
            SetFilterMask(ref _Left, mask);
            SetFilterMask(ref _Ground, mask);
            SetFilterMask(ref _Ceiling, mask);
            SetFilterMask(ref _Layer, mask);
        }
        protected void SetFilterMask(ref ContactFilter2D filter, LayerMask mask)
        {
            filter.SetLayerMask(mask);
        }

        protected void SetFilterAngles(ref ContactFilter2D filter, float originAngle, float offsetAngle)
        {
            filter.SetNormalAngle(
                originAngle - offsetAngle,
                originAngle + offsetAngle
                );
        }
    }
}