using Blaster.Interfaces;
//using Blaster.Player;
using Blaster.Bullet;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Weapon
{
    public class WeaponController : IFireable
    {
        private WeaponView weaponView;
        private BulletPool bulletPool;
        private BulletService bulletService;
        private float fireRate;
        private WeaponSO weaponSO;
        private List<Transform> targetInRange;
        public bool CanFire => throw new NotImplementedException();

        public WeaponController(WeaponSO weaponSO, Transform container)
        {
            this.weaponSO = weaponSO;
            this.weaponView = GameObject.Instantiate(weaponSO.WeaponView, container);

        }
        public void Init(BulletService bulletService)
        {
            this.bulletService = bulletService;
            bulletPool = bulletService.BulletPool;
        }
        public void Fire(Vector2 fireDirection)
        {
            BulletController bulletToFire = bulletPool.GetBullet();
            bulletToFire.ConfigureBullet(weaponView.GunPoint.position, fireDirection.normalized);
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void Rotate(Vector2 direction)
        {
            // Ensure the direction vector has a magnitude > 0 to avoid errors
            if (direction.sqrMagnitude > 0)
            {
                // Calculate the angle in degrees
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Apply the angle to the weapon's rotation
                float xDeg = 0, yDeg = 0;
                weaponView.transform.rotation = Quaternion.Euler(xDeg, yDeg, angle);


            }
        }

        public void UpdateWeapon()
        {
            if (targetInRange.Count > 0)
            {
                Rotate( targetInRange[0].position - weaponView.gameObject.transform.position);
                ShootAtTarget(targetInRange[0]);
            }
        }
        private void ShootAtTarget(Transform targetEnemy)
        {
           fireRate  -= Time.deltaTime;
            if (fireRate <= 0)
            {
                Fire(targetEnemy.position);
                ResetAttackTimer();
            }
        }
        public void SetParent(Transform parent)
        {
            weaponView.transform.parent = parent;
        }
        private void ResetAttackTimer() => fireRate = weaponSO.FireRate;
    }
}
