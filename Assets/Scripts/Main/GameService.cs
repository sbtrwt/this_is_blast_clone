using Blaster.Bullet;
using Blaster.Grid;
using Blaster.Weapon;
using System.Collections;
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
        #endregion

        #region GameObjects
        [SerializeField] private TileView _tilePrefab;
        [SerializeField] private Transform _gridContainer;
        #endregion
        // Start is called before the first frame update
        private void Start()
        {
            CreateServices();
        }

       private void CreateServices()
        {
            _gridService = new GridService(5, 5, _tilePrefab, _gridContainer);
        }

        private void InjectDependencies() { }
        // Update is called once per frame
        private void Update()
        {

        }
    }
}