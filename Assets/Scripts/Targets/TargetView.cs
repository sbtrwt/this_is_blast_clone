using Blaster.Interfaces;
using UnityEngine;

namespace Blaster.Target
{
    public class TargetView : MonoBehaviour, IDamageable
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] protected MeshRenderer _meshRenderer;

        public TargetController Controller;

        public void TakeDamage(float damageToTake) => Controller?.TakeDamage(damageToTake);
        public virtual void Setcolor(Color color) 
        { 
            _spriteRenderer.color = color; 
            _meshRenderer.material.color = color;
        }
        public void PlayDestroyAnimation()
        {
          animator.Play("Destroy");
        }
    }
}
