using UnityEngine;

namespace Project.Controller2D.Player
{
    public abstract class PlayerController2D : EntityController2D<IGroundSensorPlayer>
    {
        #region Values

        [SerializeField] private PlayerController2DSettings _playerSettings;

        protected PlayerController2DData _playerData { get; private set; }

        #endregion

        #region Mono and inherited methods

        protected override void CreateData()
        {
            base.CreateData();
            _playerData = new PlayerController2DData(_playerSettings);
        }

        #endregion

        #region Controller logic

        public override void Jump()
        {
            _inputData.GiveJumpInput();
        }

        public override void Move(int moveDirectionSign)
        {
            _inputData.GiveMoveInput(moveDirectionSign);
        }

        #endregion
    }
}