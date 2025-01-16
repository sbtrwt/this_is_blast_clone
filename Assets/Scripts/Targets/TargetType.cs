using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Targets
{
    [CreateAssetMenu(fileName = "TargetType", menuName = "Targets/TargetType")]
    public  class TargetType : ScriptableObject
    {
        public Sprite Sprite;
        public Color Color;
    }

}
