using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Bullet
{
    public class BulletView : MonoBehaviour
    {
        public BulletController Controller;
        private void Update() => Controller?.UpdateBulletMotion();

        private void OnTriggerEnter2D(Collider2D collision) => Controller?.OnBulletEnteredTrigger(collision.gameObject);
    }
}
