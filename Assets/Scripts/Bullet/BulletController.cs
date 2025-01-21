
using Blaster.Interfaces;
using UnityEngine;

namespace Blaster.Bullet
{
    public class BulletController
    {
        private BulletView _bulletView;
        private BulletSO _bulletSO;
        private Vector2 _bulletDirection;
        private IObjectPoolHandler<BulletController> _objectPoolHandler;
        private Vector2 _minBound = new Vector2(-10,-10);
        private Vector2 _maxBound = new Vector2(10, 10);
        private Vector2 _target;
        public BulletController( BulletView bulletView, Transform bulletContainer, BulletSO bulletSO, IObjectPoolHandler<BulletController> objectPoolHandler)
        {
            this._bulletView = GameObject.Instantiate(bulletView, bulletContainer);
            this._bulletSO = bulletSO;
            this._bulletView.Controller = this;
            this._objectPoolHandler = objectPoolHandler;
        }
        public void ConfigureBullet(Vector2 position, Vector2 direction, Vector2 target)
        {
            _bulletView.gameObject.SetActive(true);
            _bulletView.transform.position = position;
            _bulletDirection = direction.normalized;
            SetTarget(target);
            //bulletView.transform.rotation = spawnTransform.rotation;
        }
        public void SetTarget(Vector2 target)
        {
            this._target = target;
        }
        public void UpdateBulletMotion()
        {
            if (_bulletView.gameObject.activeSelf)
            {
                // Move the bullet in the direction
                _bulletView.transform.Translate(Time.deltaTime * _bulletSO.Speed * _bulletDirection);

                // Check if the bullet has reached the target
                if (Vector2.Distance(_bulletView.transform.position, _target) <= 0.5f)
                {

                    _bulletView.gameObject.SetActive(false);
                    _objectPoolHandler.ReturnItem(this);
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
            if (_bulletView.transform.position.x > _maxBound.x || _bulletView.transform.position.x < _minBound.x || _bulletView.transform.position.y > _maxBound.y || _bulletView.transform.position.y < _minBound.y)
            {
                _bulletView.gameObject.SetActive(false);
                _objectPoolHandler.ReturnItem(this);
            }
        }
    }
}
