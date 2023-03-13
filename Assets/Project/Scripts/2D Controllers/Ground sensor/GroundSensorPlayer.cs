using System;
using UnityEngine;

namespace Project.Controller2D
{
    public class GroundSensorPlayer : MonoBehaviour, IGroundSensorPlayer
    {
        [SerializeField] private FiltersHolderPlayer _FilterHolder;
        protected CapsuleCollider2D _itsCapsule;

        private GroundData _cachedGroundData = new GroundData();
        private bool _hasBeenRefreshed;

        public event Action OnDataChanged;

        public FiltersHolderPlayer Filters => _FilterHolder;
        public bool Ground => _cachedGroundData.Ground;
        public bool Slope => _cachedGroundData.Slope;
        public bool GroundOrSlope => Ground || Slope;
        public bool Ceiling => _cachedGroundData.Ceiling;
        public bool Right => _cachedGroundData.Right;
        public bool Left => _cachedGroundData.Left;
        public bool Anything => _cachedGroundData.Anything;

        protected virtual void Awake()
        {
            this.TryGetReference(ref _itsCapsule);
        }
        protected virtual void FixedUpdate()
        {
            _hasBeenRefreshed = false;
        }
        private void OnCollisionEnter2D(Collision2D collision) => Refresh();
        private void OnCollisionStay2D(Collision2D collision) => Refresh();
        private void OnCollisionExit2D(Collision2D collision) => Refresh();

        private void Refresh()
        {
            if (_hasBeenRefreshed) return;

            CacheData();
            _hasBeenRefreshed = true;
        }

        private void CacheData()
        {
            GroundData data = GetData();

            if (data != _cachedGroundData)
            {
                _cachedGroundData = data;
                OnDataChanged?.Invoke();
            }
        }
        protected GroundData GetData() => new GroundData
        {
            Anything = Cast(Filters.Anything),

            Ground = Cast(Filters.Ground),
            Ceiling = Cast(Filters.Ceiling),

            Left = Cast(Filters.Left),
            Right = Cast(Filters.Right),

            Slope = Cast(Filters.GroundOrSlope)
        };
        protected bool Cast(ContactFilter2D filter) => _itsCapsule.IsTouching(filter);

        #region Internal data struct

        protected struct GroundData
        {
            public bool Ground, Ceiling, Left, Right, Slope;
            private bool _anything;

            public bool Anything
            {
                get => _anything;
                set
                {
                    _anything = value;
                    if (!Anything)
                        Ground = Ceiling = Left = Right = Slope = false;
                }
            }

            public static bool operator ==(GroundData left, GroundData right) => left.Equals(right);
            public static bool operator !=(GroundData left, GroundData right) => !left.Equals(right);

            public override int GetHashCode() => base.GetHashCode();
            public override bool Equals(object obj) => base.Equals(obj);
        }

        #endregion
    }
}