using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Grid
{
    public class GenericGridService<TController, TObject>
        where TController : IGridController<TObject>
        where TObject : IGridObject
    {
        private List<Queue<TController>> _columns;
        private int _rows;
        private int _columnsCount;

        public GenericGridService(int rows, int columns)
        {
            _rows = rows;
            _columnsCount = columns;
            _columns = new List<Queue<TController>>();

            for (int i = 0; i < columns; i++)
            {
                _columns.Add(new Queue<TController>());
            }
        }

        public void CreateGrid(System.Func<int, int, TController> createControllerFunc)
        {
            for (int column = 0; column < _columnsCount; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    var controller = createControllerFunc(column, row);
                    _columns[column].Enqueue(controller);
                }
            }
        }

        public TController GetBottomController(int column)
        {
            if (column < 0 || column >= _columnsCount || _columns[column].Count == 0)
                return default;

            return _columns[column].Peek();
        }

        public void RemoveBottomItem(int column)
        {
            if (column < 0 || column >= _columnsCount || _columns[column].Count == 0)
                return;

            var bottomController = _columns[column].Peek();
            if (bottomController != null && bottomController.GetItemCount() > 0)
            {
                var removedItem = bottomController.RemoveItem();
                Debug.Log($"Removed item: {removedItem.GetTransform().name}");
            }

            // If the bottom controller has no more items, remove it from the queue
            if (bottomController.GetItemCount() == 0)
            {
                _columns[column].Dequeue();
                Debug.Log($"Removed bottom controller in column {column}");
            }

            ShiftColumn(column);
        }

        private void ShiftColumn(int column)
        {
            int rowIndex = 0;
            foreach (var controller in _columns[column])
            {
                controller.SetPosition(new Vector2(column, rowIndex));
                rowIndex++;
            }
        }
    }
}
