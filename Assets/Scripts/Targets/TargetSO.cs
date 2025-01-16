using Assets.Scripts.Targets;
using Blaster.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
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
