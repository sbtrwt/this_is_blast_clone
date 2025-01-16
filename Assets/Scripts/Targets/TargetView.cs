using Blaster.Interfaces;
using UnityEngine;

namespace Blaster.Target
{
    public class TargetView : MonoBehaviour, IDamageable
    {
        public TargetController Controller;

        public void TakeDamage(float damageToTake) => Controller?.TakeDamage(damageToTake);
    }
}
