﻿using Blaster.Bullet;
using Blaster.Interfaces;
using Blaster.Weapon;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponController : IFireable
{
    private WeaponView _weaponView;
    private BulletPool _bulletPool;
    private BulletService _bulletService;
    private float _fireRate;
    private WeaponSO _weaponSO;
    private List<Transform> _targetsInRange = new List<Transform>();
    private Transform _target;
    private bool _isActive=false;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    public bool CanFire => _isActive && _fireRate <= 0;

    public WeaponController(WeaponSO weaponSO, Transform container)
    {
        this._weaponSO = weaponSO;
        this._weaponView = GameObject.Instantiate(weaponSO.WeaponView, container);
        _weaponView.Controller = this;
        _fireRate = weaponSO.FireRate; // Initialize fireRate
    }

    public void Init(BulletService bulletService)
    {
        this._bulletService = bulletService;
        _bulletPool = bulletService.BulletPool;
    }

    public void Fire(Vector2 fireDirection)
    {
        if (CanFire)
        {
            BulletController bulletToFire = _bulletPool.GetBullet();
            bulletToFire.ConfigureBullet(_weaponView.GunPoint.position, fireDirection.normalized);
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
            _weaponView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void RotateTowardsTarget()
    {
        if (_target != null)
        {
            Vector3 direction = _target.position - _weaponView.GunPoint.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _weaponView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void Update()
    {
        if (_targetsInRange != null && _targetsInRange.Count > 0)
        {
            _target = _targetsInRange[0];

            if (_target != null)
            {
                RotateTowardsTarget();
                ShootAtTarget(_target);
            }
        }
        else
        {
            _target = null;
        }

        _fireRate -= Time.deltaTime; // Decrease fireRate over time
    }

    private void ShootAtTarget(Transform targetEnemy)
    {
        if (CanFire && targetEnemy != null)
        {
            Vector2 fireDirection = (targetEnemy.position - _weaponView.GunPoint.transform.position).normalized;
            Fire(fireDirection);
        }
    }
    public void RemoveTarget(Transform target)
    {
        _targetsInRange.Remove(target);
    }

    public void SetParent(Transform parent)
    {
        _weaponView.transform.parent = parent;
    }

    private void ResetAttackTimer() => _fireRate = _weaponSO.FireRate;

    public void SetTargetInRange(List<Transform> targets)
    {
        _targetsInRange = targets;
    }
    public void AddTarget(Transform target)
    {
        _targetsInRange.Add(target);
    }
}
