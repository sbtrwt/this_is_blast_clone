using Blaster.Bullet;
using Blaster.Events;
using Blaster.Sound;
using Blaster.Target;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blaster.Weapon
{
    public class WeaponService
    {
        private List<WeaponController> weapons = new List<WeaponController>();

        private BulletService _bulletService;
        private SoundService _soundService;

        private WeaponSO _weaponSO;
        private Transform _weaponContainer;
        private EventService _eventService;
        private WeaponHolderService _weaponHolderService;
        private List<TargetController> _targetControllers = new List<TargetController>();
        private List<ShooterStageController> _stages = new List<ShooterStageController>();
        public WeaponService(WeaponSO weaponSO, Transform container)
        {
            this._weaponSO = weaponSO;

            this._weaponContainer = container;
        }
        public void Init(BulletService bulletService, EventService eventService, WeaponHolderService weaponHolderService, SoundService soundService)
        {
            this._bulletService = bulletService;
            this._eventService = eventService;
            this._weaponHolderService = weaponHolderService;
            this._soundService = soundService;
            //CreateWeapon(_weaponSO, _weaponContainer);
            SubscribeToEvents();
            //CreateStage(2);
            //_weaponHolderService.FillIntoWeaponHolder(_weaponSO, _bulletService, this);

        }

        public void SubscribeToEvents()
        {
            _eventService.OnTargetLoaded.AddListener(SetTargetInRange);
            _eventService.OnNewColumnTarget.AddListener(AddTarget);
            _eventService.OnTargetRemoved.AddListener(RemoveTarget);

        }
        public void unSubscribeToEvents()
        {
            _eventService.OnTargetLoaded.RemoveListener(SetTargetInRange);
            _eventService.OnNewColumnTarget.RemoveListener(AddTarget);
            _eventService.OnTargetRemoved.RemoveListener(RemoveTarget);
        }
        public WeaponController CreateWeapon(WeaponSO weaponSO, Transform container)
        {
            this._weaponSO = weaponSO;
            WeaponController weaponController = new WeaponController(weaponSO, container, this);
            weaponController.Init(_bulletService, _weaponHolderService);
            weapons.Add(weaponController);
            return weaponController;
        }
        public List<WeaponController> GetWeapons()
        {
            return weapons;
        }
        public void SetTargetInRange(List<TargetController> targets)
        {
            Debug.Log("Set Target");
            _targetControllers = new List<TargetController>();
            foreach (var target in targets)
            {
                if (target != null)
                    _targetControllers.Add(target);
            }
            UpdateTargets();
        }
        public void AddTarget(TargetController target)
        {
            Debug.Log("Add Target");
            if (target != null)
                _targetControllers.Add(target);
            UpdateTargets();
        }
        public void RemoveTarget(TargetController target)
        {
            Debug.Log("Remove Target");
            if (target != null)
            {
                _targetControllers.Remove(target);
                foreach (var stage in _stages)
                {
                    stage.WeaponController?.RemoveTarget(target);
                }
            }

        }
        public void Update()
        {

            foreach (var stage in _stages)
            {
                stage.WeaponController?.Update();
            }
        }
        public void SetWeaponToActive()
        {
            weapons.FirstOrDefault().IsActive = true;
        }
        public void AddWeapon(WeaponController weapon)
        {
            weapons.Add(weapon);
        }
        public void CreateStage(int count, ShooterStageView stagePrefab)
        {
            RemoveStage();
            _stages = new List<ShooterStageController>();
            for (int i = 0; i < count; i++)
            {
                ShooterStageController stage = new ShooterStageController(stagePrefab, _weaponContainer);
                stage.IsActive = true;
                stage.IsFilled = false;
                stage.Position = new Vector2(i, 0);
                stage.SetLocalPosition(stage.Position);
                _stages.Add(stage);
            }
        }
        public void RemoveStage()
        {
            if (_stages != null && _stages.Count > 0)
            {
                for (int i = 0; i < _stages.Count; i++)
                {
                    if (_stages[i].IsFilled == true)
                    {
                        _stages[i].WeaponController.IsActive = false;
                        _stages[i].WeaponController.DestroyWeapon();
                        
                    }
                }
            }
        }
        public ShooterStageController GetEmtpyWeaponStage()
        {
            return _stages.FirstOrDefault(x => x.IsFilled == false);
        }
        public bool IsAllStagesFilled()
        {
            return _stages.All(x => x.IsFilled == true);
        }
        public void FillWeaponStage(WeaponController weapon)
        {
            var stage = GetEmtpyWeaponStage();
            Debug.Log("Stage " + stage);
            if (stage != null)
            {
                stage.WeaponController = weapon;
                stage.IsFilled = true;
                weapon.IsActive = true;
                weapon.SetWeaponContainer(_weaponContainer);
                weapon.SetLocalPosition(stage.Position);
                weapon.SetTargetInRange(_targetControllers);
            }
        }
        public void UpdateTargets()
        {
            foreach (var stage in _stages)
            {
                stage.WeaponController?.SetTargetInRange(_targetControllers);
            }
        }

        public void RemoveWeaponFromStage(WeaponController weapon)
        {
            var stage = _stages.FirstOrDefault(x => x.WeaponController == weapon);
            if (stage != null)
            {
                stage.WeaponController = null;
                stage.IsFilled = false;
                weapon.IsActive = false;
            }
        }
        public void OnWeaponFire()
        {
            _soundService.PlaySoundEffects(SoundType.Shoot);
        }
        ~WeaponService()
        {
            unSubscribeToEvents();
        }
    }
}
