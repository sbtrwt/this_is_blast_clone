using Blaster.Bullet;
using Blaster.Interfaces;
using Blaster.Weapon;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponController : IFireable
{
    private WeaponView weaponView;
    private BulletPool bulletPool;
    private BulletService bulletService;
    private float fireRate;
    private WeaponSO weaponSO;
    private List<Transform> targetsInRange = new List<Transform>();
    private Transform target;
    public bool CanFire => fireRate <= 0;

    public WeaponController(WeaponSO weaponSO, Transform container)
    {
        this.weaponSO = weaponSO;
        this.weaponView = GameObject.Instantiate(weaponSO.WeaponView, container);
        fireRate = weaponSO.FireRate; // Initialize fireRate
    }

    public void Init(BulletService bulletService)
    {
        this.bulletService = bulletService;
        bulletPool = bulletService.BulletPool;
    }

    public void Fire(Vector2 fireDirection)
    {
        if (CanFire)
        {
            BulletController bulletToFire = bulletPool.GetBullet();
            bulletToFire.ConfigureBullet(weaponView.GunPoint.position, fireDirection.normalized);
            ResetAttackTimer();
        }
    }

    public void Reload()
    {
        throw new NotImplementedException();
    }

    public void Rotate(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - weaponView.GunPoint.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void Update()
    {
        if (targetsInRange != null && targetsInRange.Count > 0)
        {
            target = targetsInRange[0];

            if (target != null)
            {
                RotateTowardsTarget();
                ShootAtTarget(target);
            }
        }
        else
        {
            target = null;
        }

        fireRate -= Time.deltaTime; // Decrease fireRate over time
    }

    private void ShootAtTarget(Transform targetEnemy)
    {
        if (CanFire && targetEnemy != null)
        {
            Vector2 fireDirection = (targetEnemy.position - weaponView.GunPoint.transform.position).normalized;
            Fire(fireDirection);
        }
    }
    public void RemoveTarget(Transform target)
    {
        targetsInRange.Remove(target);
    }

    public void SetParent(Transform parent)
    {
        weaponView.transform.parent = parent;
    }

    private void ResetAttackTimer() => fireRate = weaponSO.FireRate;

    public void SetTargetInRange(List<Transform> targets)
    {
        targetsInRange = targets;
    }
    public void AddTarget(Transform target)
    {
        targetsInRange.Add(target);
    }
}
