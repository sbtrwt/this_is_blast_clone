using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Bullet
{
    [CreateAssetMenu(fileName = "BulletSO", menuName = "ScriptableObjects/BulletSO")]
    public class BulletSO:ScriptableObject 
    {
        public float Speed;
        public float Damage;
        public BulletView BulletView;
    }
}
