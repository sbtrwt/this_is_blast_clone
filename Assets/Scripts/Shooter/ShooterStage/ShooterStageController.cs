using UnityEngine;

namespace Blaster.Weapon
{
    public class ShooterStageController
    {
        private ShooterStageView _shooterStageView;
        public bool IsFilled { get; set; }
        public bool IsActive { get; set; }
        public WeaponController WeaponController { get; set; }
        public Vector2 Position { get; set; }

        public ShooterStageController(ShooterStageView shooterStageView, Transform container)
        {
            _shooterStageView = GameObject.Instantiate( shooterStageView , container);
            _shooterStageView.Controller = this;
        }
        public void SetLocalPosition(Vector2 position)
        {
            _shooterStageView.transform.localPosition = position;
        }
    }
}
