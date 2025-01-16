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
        public int Rows;
        public int Columns;
        public TileView TileView;
        //public TargetType[,] TargetTypes;
        public List<TargetData> TargetTypes;
        public int ShooterRows;
        public int ShooterColumns;
        public List<ShooterData> ShooterTypes;
        //public WeaponSO[,] ShooterTypes;
    }

    [Serializable]
    public struct ShooterData
    {
        public int X, Y;
        public WeaponSO WeaponSO;
    }
    [Serializable]
    public struct TargetData
    {
        public int X, Y;
        public TargetType TargetType;
    }
}
