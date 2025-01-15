using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Weapon
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        public WeaponView WeaponView;
        public float FireRate;  
    }
}
