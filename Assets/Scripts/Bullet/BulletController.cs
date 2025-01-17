
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
        public BulletController( BulletView bulletView, Transform bulletContainer, BulletSO bulletSO, IObjectPoolHandler<BulletController> objectPoolHandler)
        {
            this.bulletView = GameObject.Instantiate(bulletView, bulletContainer);
            this.bulletSO = bulletSO;
            this.bulletView.Controller = this;
            this.objectPoolHandler = objectPoolHandler;
        }
        public void ConfigureBullet(Vector2 position, Vector2 direction)
        {
            bulletView.gameObject.SetActive(true);
            bulletView.transform.position = position;
            bulletDirection = direction.normalized;
            //bulletView.transform.rotation = spawnTransform.rotation;
        }
        public void UpdateBulletMotion()
        {
            //Debug.Log("bulletSO.Speed: " + bulletSO.Speed);
            //Debug.Log("bulletDirection: " + bulletDirection);
            bulletView.transform.Translate( Time.deltaTime * bulletSO.Speed * bulletDirection);
            CheckBulletOutOfScreen();
        }
        public void OnBulletEnteredTrigger(GameObject collidedGameObject)
        {
            if (collidedGameObject.GetComponent<IDamageable>() != null)
            {
                collidedGameObject.GetComponent<IDamageable>().TakeDamage(bulletSO.Damage);
                //GameService.Instance.GetSoundService().PlaySoundEffects(SoundType.BulletHit);
                //GameService.Instance.GetVFXService().PlayVFXAtPosition(VFXType.BulletHitExplosion, bulletView.transform.position);
                
                bulletView.gameObject.SetActive(false);
                objectPoolHandler.ReturnItem(this);
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
