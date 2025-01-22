using Blaster.Bullet;
using Blaster.Events;
using Blaster.Sound;
using Blaster.Target;
using System.Collections;
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
        private GameService _gameService;

        private WeaponSO _weaponSO;
        private Transform _weaponContainer;
        private EventService _eventService;
        private WeaponHolderService _weaponHolderService;
        private List<TargetController> _targetControllers = new List<TargetController>();
        private List<ShooterStageController> _stages;
        public WeaponService(WeaponSO weaponSO, Transform container)
        {
            this._weaponSO = weaponSO;

            this._weaponContainer = container;
        }
        public void Init(BulletService bulletService, EventService eventService, WeaponHolderService weaponHolderService, SoundService soundService, GameService gameService)
        {
            this._bulletService = bulletService;
            this._eventService = eventService;
            this._weaponHolderService = weaponHolderService;
            this._soundService = soundService;
            this._gameService = gameService;
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
            //UpdateTargets();
        }
        public void AddTarget(TargetController target)
        {
            //Debug.Log("Add Target");
            //Debug.Log(target);
            if (target != null && !(_targetControllers.Contains(target)))
                _targetControllers.Add(target);
            //UpdateTargets();
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
            UpdateTargets();
            foreach (var stage in _stages)
            {
                stage.WeaponController?.Update();
            }
            IsGameOver();
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

            // Calculate the total width and starting position
            float stageWidth = stagePrefab.transform.localScale.x; // Adjust if prefab size isn't tied to scale
            float totalWidth = count * stageWidth;
            float startX = -(totalWidth / 2) + (stageWidth / 2); // Center-align the stages

            for (int i = 0; i < count; i++)
            {
                // Create a new stage
                ShooterStageController stage = new ShooterStageController(stagePrefab, _weaponContainer);
                stage.IsActive = true;
                stage.IsFilled = false;

                // Calculate the position of each stage
                float xPos = startX + (i * stageWidth);
                Vector3 position = new Vector3(xPos, 0, 0); // Centered horizontally, aligned vertically

                // Set position relative to parent
                stage.Position = position;
                stage.SetLocalPosition(position);

                // Add the stage to the list
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
                        _stages[i].WeaponController = null;
                    }
                        _stages[i].Destroy();
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
                //weapon.IsActive = true;
                weapon.SetWeaponContainer(_weaponContainer);
                _soundService.PlaySoundEffects(SoundType.StageShooter);
                // Start the animation coroutine
                Vector3 targetPosition = stage.Position;
               _gameService. StartCoroutine(MoveWeaponToPosition(weapon, targetPosition, 0.1f)); // Adjust duration as needed
                weapon.SetTargetInRange(_targetControllers);

            }
        }
        private IEnumerator MoveWeaponToPosition(WeaponController weapon, Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = weapon.GetLocalPosition();
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Smooth interpolation
                weapon.SetLocalPosition(Vector3.Lerp(startPosition, targetPosition, t));
                yield return null;
            }
            weapon.IsActive = true;
            // Ensure the final position is set
            weapon.SetLocalPosition(targetPosition);
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
          _gameService.Invoke(nameof( Vibrate), 0.1f);
        }
        public void OnWeaponExit()
        {
            _soundService.PlaySoundEffects(SoundType.Exit);
        }
        private void Vibrate() { Handheld.Vibrate(); }
        public void IsGameOver()
        {
            if(IsAllStagesFilled() && IsStageAllWeaponIdle())
            {
                _eventService.OnGameEnd.InvokeEvent(false);
            }
        }
        public bool IsStageAllWeaponIdle()
        {
            bool isAllIdle = true;

            foreach (var stage in _stages)
            {
                if (stage.WeaponController != null && stage.WeaponController.IsWeaponIdleFor(0.1f))
                {
                    isAllIdle = true;
                }
                else
                {
                    isAllIdle = false;
                    break;
                }
            }
            return isAllIdle;
        }


        public void ShowHandHelp(bool show)
        {
            foreach (var weapon in weapons)
            {
                weapon.ShowHandHelp(show);
            }
        }

        ~WeaponService()
        {
            unSubscribeToEvents();
        }
    }
}
