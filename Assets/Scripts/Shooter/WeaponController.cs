using Blaster.Bullet;
using Blaster.Interfaces;
using Blaster.Target;
using Blaster.Targets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<TargetController> _targetsInRange;
        private TargetController _target;
        private bool _isActive = false;
        private bool _hasFired = false; // New flag for single fire
        private WeaponState _currentWeaponState;
        private int _bulletCount;
        private float _idleTimer = 0f; // To track idle time
        private Transform _outTarget;
        private TargetType _targetType => _weaponSO.TargetType;

        public WeaponState CurrentWeaponState { get => _currentWeaponState; }
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }
        public bool CanFire => _isActive && _fireRate <= 0 && _bulletCount > 0 && !_hasFired; // Include HasFired condition

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
            _weaponView.SetHitText(_weaponSO.BulletCount.ToString());
            _outTarget = _weaponView.OutTarget;
        }

        public void Init(BulletService bulletService, WeaponHolderService weaponHolderService)
        {
            _weaponHolderService = weaponHolderService;
            _bulletService = bulletService;
            _bulletPool = bulletService.BulletPool;
        }

        public void Fire(Vector2 fireDirection)
        {
            if (CanFire )
            {
                BulletController bulletToFire = _bulletPool.GetBullet();
                bulletToFire.ConfigureBullet(_weaponView.GunPoint.position, fireDirection.normalized, _target.GetTransform().position);
                _bulletCount--;
                _hasFired = true; // Set HasFired to true after firing
                _weaponView.SetHitText(_bulletCount.ToString());
                _weaponView.PlaySmokeParticle();
                _weaponService.OnWeaponFire();
                _target.TakeDamage(1);
               
              //_target.TargetHitCounter++;
              //  if (_target.TargetHitCounter > 1)
              //  Debug.Log("Target Hit Counter: " + _target.TargetHitCounter);

                //_target.IsTargetGotHit = true;
                if (_bulletCount == 0)
                {
                    _weaponService.RemoveWeaponFromStage(this);
                    UnlockTargets();
                    DestroyWeapon();
                }
                ResetAttackTimer();
                _target = null;
            }
        }

        public void DestroyWeapon()
        {
            UnlockTargets();
            Vector3 targetPosition = _outTarget.position;
            _weaponService.OnWeaponExit();
            _weaponView.StartCoroutine(MoveToTargetAndDestroy(targetPosition));
            //GameObject.Destroy(_weaponView.gameObject);
        }
        private IEnumerator MoveToTargetAndDestroy(Vector3 targetPosition, float duration = 0.2f)
        {
            Vector3 startPosition = _weaponView.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Smoothly interpolate position
                _weaponView.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            // Ensure the final position is set
            _weaponView.transform.position = targetPosition;

            // Unlock targets and destroy the weapon
            //UnlockTargets();
            GameObject.Destroy(_weaponView.gameObject);
        }

        private void UnlockTargets()
        {
            foreach (var target in _targetsInRange)
            {
                if (target.TargetLockedBy == this)
                {
                    target.IsLocked = false;
                    target.TargetLockedBy = null;
                }
            }
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
                Vector3 direction = _target.GetTransform().position - _weaponView.GunPoint.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _weaponView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        public void Update()
        {
            if (_weaponView == null || !_isActive || _currentWeaponState == WeaponState.Shooting) return;

            if (_targetsInRange != null && _targetsInRange.Count > 0)
            {
                _target = _targetsInRange.FirstOrDefault(x => !x.IsTargetGotHit && (!x.IsLocked || x.TargetLockedBy == this) );

                if (_target != null)
                {
                    RotateTowardsTarget();
                    if (!_hasFired) // Prevent repeated firing
                    {
                        ShootAtTarget(_target);
                    }
                }
                else
                {
                    ResetFireState();
                }
            }
            else
            {
                ResetFireState();
            }

            _fireRate -= Time.deltaTime; // Decrease fireRate over time
        }

        private void ShootAtTarget(TargetController targetEnemy)
        {
            if (CanFire && targetEnemy != null && targetEnemy.IsActive && !targetEnemy.IsTargetGotHit)
            {
                targetEnemy.IsLocked = true;
                targetEnemy.TargetLockedBy = this;
               // Mark the target as hit
                _currentWeaponState = WeaponState.Shooting;

                // Calculate fire direction
                Vector2 fireDirection = (targetEnemy.GetTransform().position - _weaponView.GunPoint.transform.position).normalized;

                // Fire at the target
                Fire(fireDirection);
                //targetEnemy.IsTargetGotHit = true;
            }
        }

        private void ResetFireState()
        {
            _currentWeaponState = WeaponState.Idle;
            _hasFired = false; // Reset HasFired when the target changes or becomes invalid
            //_idleTimer = 0f;
        }

        public void RemoveTarget(TargetController target)
        {
            target.IsActive = false;
            target.IsLocked = false;
            target.TargetLockedBy = null;
            _targetsInRange.Remove(target);
            if(_target == target)
            {
                _target = null;
            }
            ResetFireState();
        }

        private void ResetAttackTimer() => _fireRate = _weaponSO.FireRate;

        public void SetTargetInRange(List<TargetController> targets)
        {
            foreach (var target in targets)
            {
                if (!_targetsInRange.Contains(target) && target.TargetType == _targetType && !target.IsTargetGotHit)
                {
                    _targetsInRange.Add(target);
                }
            }
        }

        public void SetLocalPosition(Vector2 position)
        {
            _weaponView.transform.localPosition = position;
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
        public bool IsWeaponIdleFor(float duration)
        {
            // Check if the weapon is in Idle state, has no targets, and no current target
            if (_currentWeaponState == WeaponState.Idle && _targetsInRange.Count == 0 && _target == null)
            {
                _idleTimer += Time.deltaTime; // Increment the idle timer
                return _idleTimer >= duration; // Return true if idle for the specified duration
            }
            _idleTimer = 0f; // Reset the timer if not idle
            return false;
        }
        public void Reload()
        {
            throw new System.NotImplementedException();
        }
        public Vector3 GetLocalPosition()
        {
            return _weaponView.transform.localPosition;
        }
    }

    public enum WeaponState
    {
        Waiting,
        Shooting,
        Idle
    }
}
