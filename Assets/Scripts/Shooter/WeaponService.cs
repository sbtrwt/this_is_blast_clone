using Blaster.Interfaces;
using Blaster.Bullet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Weapon
{
    public class WeaponService
    {
        private List<WeaponController> weapons = new List<WeaponController>();

        private BulletService bulletService;
        private WeaponSO weaponSO;
        private Transform weaponContainer;
        public WeaponService(WeaponSO weaponSO, Transform container)
        {
            this.weaponSO = weaponSO;
            this.weaponContainer = container;
           
        }
        public void Init(BulletService bulletService)
        {
            this.bulletService = bulletService;
            CreateWeapon(weaponSO, weaponContainer);
        }
        public WeaponController CreateWeapon(WeaponSO weaponSO, Transform container)
        {
            this.weaponSO = weaponSO;
            WeaponController weaponController = new WeaponController(weaponSO, container);
            weaponController.Init(bulletService);
            weapons.Add(weaponController);
            return weaponController;
        }
        public List<WeaponController> GetWeapons()
        {
            return weapons;
        }



    }
}
