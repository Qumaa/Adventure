using UnityEngine;
using Project.Controller2D;

namespace Project.Controller2D
{
    public struct GroundData
    {
        public bool Ground { get; private set; }
        public bool Ceiling { get; private set; }
        public bool Right { get; private set; }
        public bool Left { get; private set; }
        public bool Anything { get; private set; }

        public GroundData(bool Ground, bool Ceiling, bool Right, bool Left, bool Anything)
        {
            this.Ground = Ground;
            this.Ceiling = Ceiling;
            this.Right = Right;
            this.Left = Left;
            this.Anything = Anything;
        }

        public GroundData(Collider2D collider, FiltersHolderBase holder) => this = FromFilters(collider, holder);

        public static GroundData FromFilters(Collider2D collider, FiltersHolderBase holder)
        => new GroundData(
            collider.IsTouching(holder.Ground),
            collider.IsTouching(holder.Ceiling),
            collider.IsTouching(holder.Right),
            collider.IsTouching(holder.Left),
            collider.IsTouching(holder.Anything)
            );
        public static GroundData FromFilters(Rigidbody2D rigidbody, FiltersHolderBase holder)
        => new GroundData(
            rigidbody.IsTouching(holder.Ground),
            rigidbody.IsTouching(holder.Ceiling),
            rigidbody.IsTouching(holder.Right),
            rigidbody.IsTouching(holder.Left),
            rigidbody.IsTouching(holder.Anything)
            );
    }
}
