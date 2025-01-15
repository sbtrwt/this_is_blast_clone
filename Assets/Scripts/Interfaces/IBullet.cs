using UnityEngine;

namespace Blaster.Interfaces
{
    public interface IBullet
    {
        public void UpdateBulletMotion();

        public void OnBulletEnteredTrigger(GameObject collidedObject);
    }
}
