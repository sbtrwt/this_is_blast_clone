using Blaster.Target;
using UnityEngine;

namespace Blaster.Targets
{
    [CreateAssetMenu(fileName = "TargetSO", menuName = "Targets/TargetSO")]
    public class TargetSO : ScriptableObject
    {
        public int Health;
        public int Score;
        public TargetType TargetType;
        public TargetView TargetPrefab;
    }
}
