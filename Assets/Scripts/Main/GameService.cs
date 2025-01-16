using Blaster.Bullet;
using Blaster.Events;
using Blaster.Grid;
using Blaster.Target;
using Blaster.Targets;
using Blaster.Weapon;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Blaster
{
    public class GameService : MonoBehaviour
    {
        #region Services
        private GridService _gridService;
        private WeaponService _weaponService;
        private BulletService _bulletService;
        private EventService _eventService;
        private TargetService _targetService;
        private WeaponHolderService _weaponHolderService;
        #endregion

        #region SOs
        [SerializeField] private TargetSO _blockSO;
        [SerializeField] private WeaponSO _weaponSO;
        [SerializeField] private BulletSO _bulletSO;
        [SerializeField] private TargetSO _targetSO;
        #endregion
        #region GameObjects
        [SerializeField] private TileView _tilePrefab;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private Transform _weaponContainer;
        [SerializeField] private Transform _bulletContainer;
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private TargetView _targetView;
        [SerializeField] private Transform _stageContainer;
        #endregion
        // Start is called before the first frame update
        private void Start()
        {
            CreateServices();
            InjectDependencies();
        }

        private void CreateServices()
        {
            _eventService = new Events.EventService();
            _gridService = new GridService(4,4, _tilePrefab, _gridContainer);
            _weaponService = new WeaponService(_weaponSO, _stageContainer);
            _bulletService = new BulletService(_bulletSO);
            _targetService = new TargetService(_targetSO);
            _weaponHolderService = new WeaponHolderService(2,2,_weaponContainer);
        }

        private void InjectDependencies()
        {
            _weaponService.Init(_bulletService, _eventService, _weaponHolderService);
            //_weaponService.SetTargetInRange(_targets);
            _gridService.Init(_eventService, _targetService);
            _targetService.Init(_gridService);
            _weaponHolderService.Init(_weaponService);
        }
        // Update is called once per frame
        private void Update()
        {
            _weaponService.Update();
        }
    }
}