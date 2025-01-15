using Blaster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaster.Bullet
{
    public class BulletService : IObjectPoolHandler<BulletController>
    {
        private BulletPool bulletPool;
        public BulletPool BulletPool => bulletPool;
        public BulletService(BulletSO bulletSO)
        {
            bulletPool = new BulletPool(bulletSO.BulletView, bulletSO, this);

        }
        public void ReturnItem(BulletController item)
        {
            bulletPool.ReturnItem(item);
        }
    }
}
