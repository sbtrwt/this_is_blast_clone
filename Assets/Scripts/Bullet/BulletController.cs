
using Blaster.Interfaces;
using UnityEngine;

namespace Blaster.Bullet
{
    public class BulletController
    {
        private BulletView bulletView;
        private BulletSO bulletSO;
        private Vector2 bulletDirection;
        private IObjectPoolHandler<BulletController> objectPoolHandler;
        private Vector2 minBound = new Vector2(-10,-10);
        private Vector2 maxBound = new Vector2(10, 10);
        private Vector2 target;
        public BulletController( BulletView bulletView, Transform bulletContainer, BulletSO bulletSO, IObjectPoolHandler<BulletController> objectPoolHandler)
        {
            this.bulletView = GameObject.Instantiate(bulletView, bulletContainer);
            this.bulletSO = bulletSO;
            this.bulletView.Controller = this;
            this.objectPoolHandler = objectPoolHandler;
        }
        public void ConfigureBullet(Vector2 position, Vector2 direction, Vector2 target)
        {
            bulletView.gameObject.SetActive(true);
            bulletView.transform.position = position;
            bulletDirection = direction.normalized;
            SetTarget(target);
            //bulletView.transform.rotation = spawnTransform.rotation;
        }
        public void SetTarget(Vector2 target)
        {
            this.target = target;
        }
        public void UpdateBulletMotion()
        {
            if (bulletView.gameObject.activeSelf)
            {
                // Move the bullet in the direction
                bulletView.transform.Translate(Time.deltaTime * bulletSO.Speed * bulletDirection);

                // Check if the bullet has reached the target
                if (Vector2.Distance(bulletView.transform.position, target) <= 0.1f)
                {

                    bulletView.gameObject.SetActive(false);
                    objectPoolHandler.ReturnItem(this);
                    return;
                }

                // Check if the bullet is out of screen bounds
                CheckBulletOutOfScreen();
            }
        }
        public void OnBulletEnteredTrigger(GameObject collidedGameObject)
        {
            if (collidedGameObject.GetComponent<IDamageable>() != null)
            {
                //collidedGameObject.GetComponent<IDamageable>().TakeDamage(bulletSO.Damage);
              
                //bulletView.gameObject.SetActive(false);
                //objectPoolHandler.ReturnItem(this);
            }
        }
        //check out of screen
        public void CheckBulletOutOfScreen()
        {
            if (bulletView.transform.position.x > maxBound.x || bulletView.transform.position.x < minBound.x || bulletView.transform.position.y > maxBound.y || bulletView.transform.position.y < minBound.y)
            {
                bulletView.gameObject.SetActive(false);
                objectPoolHandler.ReturnItem(this);
            }
        }
    }
}
