using Blaster.Bullet;
using Blaster.Events;
using Blaster.Grid;
using Blaster.Level;
using Blaster.Sound;
using Blaster.Target;
using Blaster.Targets;
using Blaster.Weapon;
using System.Collections.Generic;
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
        private LevelService _levelService;
       
        [SerializeField] private UIService _uiService;
        #endregion

        #region SOs
        [SerializeField] private TargetSO _blockSO;
        [SerializeField] private WeaponSO _weaponSO;
        [SerializeField] private BulletSO _bulletSO;
        [SerializeField] private TargetSO _targetSO;
        [SerializeField] private GameLevel _levelSO;
        #endregion
        #region GameObjects
        [SerializeField] private TileView _tilePrefab;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private Transform _weaponContainer;
        [SerializeField] private Transform _bulletContainer;
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private TargetView _targetView;
        [SerializeField] private Transform _stageContainer;
        [SerializeField] private ParticleSystem _smokeParticle;
        #endregion

        #region Sound
        [SerializeField] private Sound.SoundService _soundService;
        [SerializeField] private Sound.SoundSO _soundSO;
        [SerializeField] private AudioSource _SFXAudioSource;
        [SerializeField] private AudioSource _bgMusicAudioSource;
        [SerializeField] private AudioSource _dialogAudioSource;
        #endregion
        // Start is called before the first frame update
        private void Start()
        {
            CreateServices();
            InjectDependencies();
            _eventService.OnGameStart.InvokeEvent(1);
        }

        private void CreateServices()
        {
            _eventService = new Events.EventService();
            _gridService = new GridService(4, 4, _tilePrefab, _gridContainer);
            _weaponService = new WeaponService(_weaponSO, _stageContainer);
            _bulletService = new BulletService(_bulletSO);
            _targetService = new TargetService(_targetSO, _smokeParticle);
            _weaponHolderService = new WeaponHolderService(2, 2, _weaponContainer);
            _levelService = new LevelService(_levelSO, _gridContainer);
            _soundService = new Sound.SoundService(_soundSO, _SFXAudioSource, _bgMusicAudioSource, _dialogAudioSource);
        }

        private void InjectDependencies()
        {
            _weaponService.Init(_bulletService, _eventService, _weaponHolderService, _soundService, this);
            //_weaponService.SetTargetInRange(_targets);
            _gridService.Init(_eventService, _targetService,this);
            _targetService.Init(_gridService, _eventService,_soundService);
            _weaponHolderService.Init(_weaponService);
            _levelService.Init(_gridService, _eventService, _weaponHolderService, _bulletService, _weaponService);
            _uiService.Init(_eventService, _levelService);
        }
        // Update is called once per frame
        private void Update()
        {
            _weaponService.Update();
        }
    }
}