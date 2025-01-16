using Blaster.Interfaces;
using Blaster.Bullet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Blaster.Events;
using System.Xml.Linq;
using Blaster.Target;

namespace Blaster.Weapon
{
    public class WeaponService
    {
        private List<WeaponController> weapons = new List<WeaponController>();

        private BulletService _bulletService;
        private WeaponSO _weaponSO;
        private Transform _weaponContainer;
        private EventService _eventService;
        private WeaponHolderService _weaponHolderService;
        private List<TargetController> targetControllers = new List<TargetController>();
        private List<WeaponStage> stages = new List<WeaponStage>();
        public WeaponService(WeaponSO weaponSO, Transform container)
        {
            this._weaponSO = weaponSO;
          
            this._weaponContainer = container;
        }
        public void Init(BulletService bulletService, EventService eventService, WeaponHolderService weaponHolderService)
        {
            this._bulletService = bulletService;
            this._eventService = eventService;
            this._weaponHolderService = weaponHolderService;
            //CreateWeapon(_weaponSO, _weaponContainer);
            SubscribeToEvents();
            CreateStage(2);
            _weaponHolderService.FillIntoWeaponHolder(_weaponSO, _bulletService, this);
        
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
            foreach(var target in targets)
            {
                targetControllers.Add(target);
            }
        }
        public void AddTarget(TargetController target)
        {
            targetControllers.Add(target);
        }
        public void RemoveTarget(TargetController target)
        {
            targetControllers.Remove(target);
        }
        public void Update()
        {
            foreach (var weapon in weapons)
            {
                weapon.Update();
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
        public void CreateStage(int count)
        {
            for (int i = 0; i < count; i++)
            {
                WeaponStage stage = new WeaponStage();
                stage.IsActive = true;
                stage.IsFilled = false;
                stage.Position = new Vector2(i, 0);
                stages.Add(stage);
            }
        }
        public WeaponStage GetEmtpyWeaponStage()
        {
            return stages.FirstOrDefault(x => x.IsFilled == false);
        }
        public void FillWeaponStage(WeaponController weapon)
        {
            var stage = GetEmtpyWeaponStage();
            Debug.Log(stage);
            if (stage != null)
            {
                stage.WeaponController = weapon;
                stage.IsFilled = true;
                weapon.IsActive = true;
                weapon.SetWeaponContainer(_weaponContainer);
                weapon.SetLocalPosition(stage.Position);
            }
        }
        ~WeaponService()
        {
            unSubscribeToEvents();
        }
    }

    public class WeaponStage
    {
        public bool IsFilled { get; set; }
        public bool IsActive { get; set; }
        public WeaponController WeaponController { get; set; }
        public Vector2 Position { get; set; }
    }
}
