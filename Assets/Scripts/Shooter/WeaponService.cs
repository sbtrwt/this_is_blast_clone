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

namespace Blaster.Weapon
{
    public class WeaponService
    {
        private List<WeaponController> weapons = new List<WeaponController>();

        private BulletService _bulletService;
        private WeaponSO _weaponSO;
        private Transform _weaponContainer;
        private EventService _eventService;
        public WeaponService(WeaponSO weaponSO, Transform container)
        {
            this._weaponSO = weaponSO;
            this._weaponContainer = container;
           
        }
        public void Init(BulletService bulletService, EventService eventService)
        {
            this._bulletService = bulletService;
            this._eventService = eventService;
            CreateWeapon(_weaponSO, _weaponContainer);
            SubscribeToEvents();
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
            WeaponController weaponController = new WeaponController(weaponSO, container);
            weaponController.Init(_bulletService);
            weapons.Add(weaponController);
            return weaponController;
        }
        public List<WeaponController> GetWeapons()
        {
            return weapons;
        }
        public void SetTargetInRange(List<Transform> targets)
        {
            foreach (var weapon in weapons)
            {
                weapon.SetTargetInRange(targets);
            }
        }
        public void AddTarget(Transform target)
        {
           weapons.FirstOrDefault().AddTarget(target);
        }
        public void RemoveTarget(Transform target)
        {
            weapons.FirstOrDefault().RemoveTarget(target);
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
        ~WeaponService()
        {
            unSubscribeToEvents();
        }
    }
}
