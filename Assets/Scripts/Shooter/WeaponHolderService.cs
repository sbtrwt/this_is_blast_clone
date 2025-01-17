using Blaster.Bullet;
using Blaster.Level;
using Blaster.Weapon;
using System.Collections.Generic;
using UnityEngine;
namespace Blaster.Weapon
{
    public class WeaponHolderService
    {
        private List<Queue<WeaponController>> _columns;
        private Transform _waitingArea;
        private int _rows;
        private int _columnsCount;
        private WeaponService _weaponService;

        public WeaponHolderService(int rows, int columns, Transform stageArea)
        {
            _waitingArea = stageArea;
            SetWeaponHolder(rows, columns);
        }
        public void SetWeaponHolder(int rows, int columns)
        {
            _rows = rows;
            _columnsCount = columns;
            // Initialize columns
            _columns = new List<Queue<WeaponController>>();
            for (int i = 0; i < _columnsCount; i++)
            {
                _columns.Add(new Queue<WeaponController>());
            }
        }
        public void Init(WeaponService weaponService)
        {
            _weaponService = weaponService;
        }
        public void AddWeapon(int column, WeaponController weapon)
        {
            if (column < 0 || column >= _columnsCount)
            {
                Debug.LogError($"Invalid column index: {column}");
                return;
            }

            // Define spacing (e.g., based on weapon prefab dimensions or fixed values)
            float columnSpacing = 1f; // Adjust as per weapon width or grid spacing
            float rowSpacing = 1f;    // Adjust as per weapon height or grid spacing

            // Calculate total grid width and height
            float gridWidth = _columnsCount * columnSpacing;
            float gridHeight = _rows * rowSpacing;

            // Calculate starting offsets to center the grid relative to the parent
            float startX = -(gridWidth / 2) + (columnSpacing / 2); // Center horizontally
            float startY = -(gridHeight / 2) + (rowSpacing / 2);   // Center vertically

            // Determine the weapon's position in the grid
            int currentRow = _columns[column].Count; // Current row index in the column
            float xPos = startX + (column * columnSpacing);
            float yPos = startY + ((_rows - currentRow - 1) * rowSpacing); // Adjust for top-down alignment

            // Set the weapon's local position relative to its parent
            Vector3 gridLocalPosition = new Vector3(xPos, yPos, 0);
            weapon.SetLocalPosition(gridLocalPosition);

            // Add the weapon to the column queue
            _columns[column].Enqueue(weapon);
        }


        public void OnWeaponClicked(int column)
        {
            if (column < 0 || column >= _columnsCount || _columns[column].Count == 0)
            {
                Debug.LogError($"Invalid column or empty column: {column}");
                return;
            }

            // Move the weapon to the stage
            WeaponController weapon = _columns[column].Dequeue();
            MoveWeaponToStage(weapon);

            // Shift the column upward
            ShiftColumnUp(column);
        }

        private void MoveWeaponToStage(WeaponController weapon)
        {
            _weaponService.FillWeaponStage(weapon);

            // Trigger shooting (you can add a delay or animation here if needed)
            //Debug.Log($"{weapon} moved to stage.");
        }

        private void ShiftColumnUp(int column)
        {
            // Define spacing (based on weapon prefab dimensions or fixed values)
            float columnSpacing = 1f; // Adjust as needed
            float rowSpacing = 1f;    // Adjust as needed

            // Calculate the total grid width and height
            float gridWidth = _columnsCount * columnSpacing;
            float gridHeight = _rows * rowSpacing;

            // Calculate the starting X and Y positions to center the grid
            float startX = -(gridWidth / 2) + (columnSpacing / 2); // Center horizontally
            float startY = -(gridHeight / 2) + (rowSpacing / 2);   // Center vertically

            int rowIndex = 0;

            // Reposition the remaining weapons in the column
            foreach (var weapon in _columns[column])
            {
                // Calculate the new local position for the weapon
                float xPos = startX + (column * columnSpacing); // Centered column position
                float yPos = startY + ((_rows - rowIndex - 1) * rowSpacing); // Top-down alignment
                Vector3 newLocalPosition = new Vector3(xPos, yPos, 0);

                // Set the weapon's local position
                weapon.SetLocalPosition(newLocalPosition);
                rowIndex++;
            }
        }


        public void ClearWeaponHolder()
        {
            if(_columns == null ) return;
            for (int i = 0; i < _columnsCount; i++)
            {
                while (_columns[i].Count > 0)
                {
                    WeaponController weapon = _columns[i].Dequeue();
                    weapon.DestroyWeapon();
                }
            }
        }
        public void FillIntoWeaponHolder( BulletService bulletService, WeaponService weaponService, List<ShooterData> shooterDatas )
        {
            for (int i = 0; i < _columnsCount; i++)
            {
                for (int k = 0; k < _rows; k++)
                {
                    var weaponSOToSet = shooterDatas.Find(p => p.X ==k && p.Y ==i);
                    WeaponController weaponController = new WeaponController(weaponSOToSet.WeaponSO, _waitingArea, weaponService);
                    weaponController.Init(bulletService, this);
                    weaponController.SetBulletCount(weaponSOToSet.BulletCount);
                    AddWeapon(i, weaponController);
                }
            }
        }

        public bool CheckWeaponInTopsAndMoveToStage(WeaponController weaponController)
        {
            if (_weaponService.IsAllStagesFilled()) return false;
            for (int column = 0; column < _columnsCount; column++)
            {
                if (_columns[column].Count == 0) continue;

                // Get the top weapon in the column
                WeaponController topWeapon = _columns[column].Peek();

                // Check if the specified weapon matches the top weapon
                if (topWeapon == weaponController)
                {
                    // Move the weapon to the stage
                    OnWeaponClicked(column);
                    return true;
                }
            }

            // If no match was found, return false
            return false;
        }
    }
}