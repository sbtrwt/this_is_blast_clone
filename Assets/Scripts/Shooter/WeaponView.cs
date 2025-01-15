using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform gunPoint;

        public WeaponController Controller;
        public Transform GunPoint => gunPoint;
    }
}