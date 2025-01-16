using Blaster.Targets;
using UnityEngine;

namespace Blaster.Weapon
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        public WeaponView WeaponView;
        public float FireRate;
        public int BulletCount;
        public TargetType TargetType;

        public bool IsTargetTypeMatch(TargetType targetType)
        {
            return TargetType == targetType;
        }
    }
}
