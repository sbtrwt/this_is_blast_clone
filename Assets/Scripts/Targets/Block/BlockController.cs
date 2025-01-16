using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaster.Targets
{
    public class BlockController
    {
        private BlockView blockView;
        private BlockSO blockSO;
        private int health;
        public BlockController(BlockSO blockSO, BlockView blockView)
        {
            this.blockSO = blockSO;
            this.blockView = blockView;
            health = blockSO.Health;
        }

        public void TakeDamage(int damage)
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
