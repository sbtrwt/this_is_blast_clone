using UnityEngine;

namespace Blaster.Grid
{
    public interface IGridObject
    {
        Transform GetTransform();
    }

    public interface IGridController<T> where T : IGridObject
    {
        int GetItemCount();
        T RemoveItem();
        void AddItem(T item);
        void SetPosition(Vector2 position);
    }
}
