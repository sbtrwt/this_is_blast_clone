using Blaster.Bullet;
using Blaster.Interfaces;
using Blaster.Target;
using Blaster.Targets;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Blaster.Weapon
{
    public class WeaponController : IFireable
    {
        private WeaponService _weaponService;
        private WeaponHolderService _weaponHolderService;
        private BulletService _bulletService;

        private WeaponView _weaponView;
        private BulletPool _bulletPool;
        private float _fireRate;
        private WeaponSO _weaponSO;
        private List<TargetController> _targetsInRange ;
        private TargetController _target;
        private bool _isActive = false;
        private WeaponState _currentWeaponState;
        private int _bulletCount;
        private TargetType _targetType => _weaponSO.TargetType;

        public WeaponState CurrentWeaponState { get => _currentWeaponState; }
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }
        public bool CanFire => _isActive && _fireRate <= 0 && _bulletCount > 0;

        public WeaponController(WeaponSO weaponSO, Transform container, WeaponService weaponService)
        {
            _targetsInRange = new List<TargetController>();
            this._weaponSO = weaponSO;
            this._weaponView = GameObject.Instantiate(weaponSO.WeaponView, container);
            _weaponView.Controller = this;
            _fireRate = weaponSO.FireRate; // Initialize fireRate
            _currentWeaponState = WeaponState.Waiting;
            _weaponService = weaponService;
            _bulletCount = weaponSO.BulletCount;
            _weaponView.SetColor(_weaponSO.TargetType.Color);
            _weaponView.SetHitText(weaponSO.BulletCount.ToString());
        }

        public void Init(BulletService bulletService, WeaponHolderService weaponHolderService)
        {
            _weaponHolderService = weaponHolderService;
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
                _bulletCount--;
                _weaponView.SetHitText(_bulletCount.ToString());
                _weaponView.PlaySmokeParticle();
                if (_bulletCount == 0)
                {
                    _weaponService.RemoveWeaponFromStage(this);
                    DestroyWeapon();
                }
            }
            
        }
        public void DestroyWeapon()
        {
            GameObject.Destroy(_weaponView.gameObject);
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
            if (_target != null && _target.GetTransform() != null)
            {
                Debug.Log(_target.GetTransform());
                Vector3 direction = _target.GetTransform().position - _weaponView.GunPoint.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _weaponView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                _weaponView.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        public void Update()
        {
            if(_weaponView == null || !IsActive) { return; }
            //_weaponView.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (_targetsInRange != null && _targetsInRange.Count > 0)
            {
                _target = _targetsInRange[0];
                Debug.Log("update _targetType : " + _targetType);
                Debug.Log("update _target :     " + _target.TargetType);
                Debug.Log("update _targetsInRange.Count : " + _targetsInRange.Count);

                if (_target != null && _target.IsActive && _target.TargetType == _targetType)
                {
                    
                    RotateTowardsTarget();
                    ShootAtTarget(_target);
                }
                else
                {
                    _currentWeaponState = WeaponState.Idle;
                }
            }
            else
            {
                _target = null;
            }

            _fireRate -= Time.deltaTime; // Decrease fireRate over time
        }

        private void ShootAtTarget(TargetController targetEnemy)
        {
            if (CanFire && targetEnemy != null && targetEnemy.IsActive && !targetEnemy.IsLocked)
            {
                targetEnemy.IsLocked = true;
                _currentWeaponState = WeaponState.Shooting;
                Vector2 fireDirection = (targetEnemy.GetTransform().position - _weaponView.GunPoint.transform.position).normalized;
                Fire(fireDirection);
            }
        }
        public void RemoveTarget(TargetController target)
        {
            _targetsInRange.Remove(target);
        }

        public void SetParent(Transform parent)
        {
            _weaponView.transform.parent = parent;
        }

        private void ResetAttackTimer() => _fireRate = _weaponSO.FireRate;

        public void SetTargetInRange(List<TargetController> targets)
        {
            Debug.Log("Setting targets in range");
            //_targetsInRange = targets;
            foreach (var target in targets)
            {
                if (_targetsInRange.Contains(target) == false)
                    AddTarget(target);
            }
            Debug.Log("_targetType : " + _targetType);
            Debug.Log("_targetsInRange.Count : " + _targetsInRange.Count);
           
        }
        public void AddTarget(TargetController target)
        {
            Debug.Log("AddTarget : " + target);
            Debug.Log(target.TargetType);
            Debug.Log(_targetType);

            if (target != null && target.IsActive && target.TargetType == _targetType)
            { _targetsInRange.Add(target); }
        }
        public void SetPosition(Vector2 position)
        {
            _weaponView.transform.position = position;
        }
        public void SetLocalPosition(Vector2 position)
        {
            _weaponView.transform.localPosition = position;
        }
        public void SetCurrentWeaponState(WeaponState weaponState)
        {
            _currentWeaponState = weaponState;
        }
        public bool CheckWeaponInTopRow()
        {
            if (CurrentWeaponState == WeaponState.Waiting)
                return _weaponHolderService.CheckWeaponInTopsAndMoveToStage(this);
            else
                return false;
        }
        public void SetWeaponContainer(Transform container)
        {
            _weaponView.transform.parent = container;
        }
        public void SetBulletCount(int bulletCount)
        {
            _bulletCount = bulletCount;
            _weaponView.SetHitText(bulletCount.ToString());
        }
    }

    public enum WeaponState
    {
        Waiting,
        Shooting,
        Idle
    }
}