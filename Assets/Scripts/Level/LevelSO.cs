using Blaster.Grid;
using Blaster.Targets;
using Blaster.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Level
{

    [CreateAssetMenu(fileName = "LevelSO", menuName = "Level/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        public int LevelIndex;
        [Range(1, 100)] public int Rows;
        [Range(1, 100)] public int Columns;
        public int StageColumns;
        public ShooterStageView ShooterStageView;
        public TileView TileView;
        //public TargetType[,] TargetTypes;
        public List<TargetData> TargetTypes;
        public int ShooterRows;
        public int ShooterColumns;
        public List<ShooterData> ShooterTypes;
        //public WeaponSO[,] ShooterTypes;


        public void OnValidate()
        {
            // Ensure TargetTypes list is initialized
            if (TargetTypes == null)
            {
                TargetTypes = new List<TargetData>();
            }

            // Ensure grid data matches the row and column settings
            EnsureTargetDataMatchesGrid();
        }

        private void EnsureTargetDataMatchesGrid()
        {
            // Add missing TargetData entries for each grid cell
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (!TargetTypes.Exists(t => t.X == column && t.Y == row))
                    {
                        TargetTypes.Add(new TargetData { X = column, Y = row, TargetType = null });
                    }
                }
            }

            // Remove TargetData entries that are outside the current grid bounds
            TargetTypes.RemoveAll(t => t.X >= Columns || t.Y >= Rows);
        }
    }

    [Serializable]
    public struct ShooterData
    {
        public int X, Y;
        public int BulletCount;
        public WeaponSO WeaponSO;
    }
    [Serializable]
    public struct TargetData
    {
        public int X, Y;
        public TargetType TargetType;
    }
}
