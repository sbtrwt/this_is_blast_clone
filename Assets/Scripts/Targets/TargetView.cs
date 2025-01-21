using Blaster.Interfaces;
using UnityEngine;

namespace Blaster.Target
{
    public class TargetView : MonoBehaviour, IDamageable
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator animator;
        public TargetController Controller;

        public void TakeDamage(float damageToTake) => Controller?.TakeDamage(damageToTake);
        public virtual void Setcolor(Color color) { _spriteRenderer.color = color; }
        public void PlayDestroyAnimation()
        {
          animator.Play("Destroy");
        }
    }
}
