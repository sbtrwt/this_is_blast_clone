using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaster.Target
{
    public class BlockController
    {
        private BlockView blockView;
        //private BlockSO blockSO;
        private float health;
        public BlockController(BlockView blockView)
        {
            
            this.blockView = blockView;
            health = 1;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                DestroyBlock();
            }
        }

        private void DestroyBlock()
        {
            UnityEngine.Object.Destroy(blockView.gameObject);
        }
    }
}
