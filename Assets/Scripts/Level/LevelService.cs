
using Blaster.Bullet;
using Blaster.Events;
using Blaster.Grid;
using Blaster.Weapon;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Blaster.Level
{
    public class LevelService
    {
        #region Services
        private GridService _gridService;
        private EventService _eventService;
        private WeaponHolderService _weaponHolderService;
        private BulletService _bulletService;
        private WeaponService _weaponService;
        #endregion


        private GameLevel _gameLevel;
        private LevelSO _currentLevel;
        private int _currentLevelIndex;
        private Transform _gridContainer;
        //private ShooterStageView _shooterStageView;
        public LevelService(GameLevel gameLevel, Transform gridContainer ) 
        { 
            _gameLevel = gameLevel;
            _gridContainer = gridContainer;
          
        }

        public void Init(GridService gridService, EventService eventService, WeaponHolderService weaponHolderService, BulletService bulletService, WeaponService weaponService )
        {

            _gridService = gridService;
            _eventService = eventService;
            _weaponHolderService = weaponHolderService;
            _bulletService = bulletService;
            _weaponService = weaponService;
            SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            _eventService.OnGameStart.AddListener(OnGameStart);
            _eventService.OnGameEnd.AddListener(OnGameEnd);
        }
        public void UnsubscribeToEvents()
        {
            _eventService.OnGameStart.RemoveListener(OnGameStart);
            _eventService.OnGameEnd.RemoveListener(OnGameEnd);
        }
        public void OnGameStart(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            LoadLevel();
            Debug.Log("Game Started");
        }
        public void OnGameEnd(bool isWin)
        {
            Debug.Log("Game Ended");
        }

        public void LoadLevel()
        {
            _currentLevel = _gameLevel.Levels.Find(level => level.LevelIndex == _currentLevelIndex);
            if(_currentLevel == null)
            {
                Debug.LogError("Level not found");
                Application.Quit();
                return;
            }
            LoadGrid();
            LoadWeapon();
        }

        private void LoadGrid()
        {
            _gridService.CleanGrid();
            _gridService.CreateGrid(_currentLevel.Rows, _currentLevel.Columns, _currentLevel.TileView, _gridContainer, _currentLevel.TargetTypes);
            _gridService.OnTargetsLoaded();
        }
        private void LoadWeapon()
        {
            _weaponService.CreateStage(_currentLevel.StageColumns, _currentLevel.ShooterStageView);
            _weaponHolderService.ClearWeaponHolder();
            _weaponHolderService.SetWeaponHolder(_currentLevel.ShooterRows, _currentLevel.ShooterColumns);
            
            _weaponHolderService.FillIntoWeaponHolder( _bulletService, _weaponService, _currentLevel.ShooterTypes);
            if (_currentLevel.IsHelp)
            { _weaponHolderService.ShowHelp(); }
        }
        public void NextLevel()
        {
            _eventService.OnGameStart.InvokeEvent(_currentLevelIndex + 1);
        }
        ~LevelService()
        {
            UnsubscribeToEvents();
        }
    }
}