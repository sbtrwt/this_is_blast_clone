﻿using Blaster.Interfaces;
using UnityEngine;

namespace Blaster.Target
{
    public class TargetView : MonoBehaviour, IDamageable
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        public TargetController Controller;

        public void TakeDamage(float damageToTake) => Controller?.TakeDamage(damageToTake);
        public virtual void Setcolor(Color color) { _spriteRenderer.color = color; }
    }
}
