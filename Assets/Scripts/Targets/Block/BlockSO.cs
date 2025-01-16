using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Blaster.Targets
{
    [CreateAssetMenu(fileName = "BlockSO", menuName = "Target/BlockSO")]
    public class BlockSO : ScriptableObject
    {
        public int Health;
        public int Score;
    }
}
