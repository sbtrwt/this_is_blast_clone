using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Level
{
    [CreateAssetMenu(fileName = "GameLevel", menuName = "Level/GameLevel")]
    public class GameLevel : ScriptableObject
    {
        public List<LevelSO> Levels;
    }
}
