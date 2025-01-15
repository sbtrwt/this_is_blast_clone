using UnityEngine;

namespace Blaster.Interfaces
{
    public interface IFireable
    {
        void Fire(Vector2 direction);
        void Reload();
        bool CanFire { get; }
    }
}
