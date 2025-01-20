
using Blaster.Events;
using Blaster.Level;
using Blaster.Target;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Grid
{
    public class GridService
    {
        private List<Queue<TileController>> _columns;
        private int _rows;
        private int _columnsCount;
        private EventService _eventService;
        private TargetService _targetService;
        private TileView _tileView;
        private Transform _container;
        public GridService(int rows, int columns, TileView tileView, Transform container)
        {
            _rows = rows;
            _columnsCount = columns;

            _tileView = tileView;
            _container = container;
        }
        public void Init(EventService eventService, TargetService targetService)
        {
            this._eventService = eventService;
            this._targetService = targetService;
            //CreateGrid(_rows, _columnsCount, _tileView, _container);
            //OnTargetsLoaded();
        }
        public void CreateGrid(int rows, int columns, TileView tileView, Transform container, List<TargetData> targetTypes)
        {
            _rows = rows;
            _columnsCount = columns;
            _columns = new List<Queue<TileController>>();
            _targetService.ResetTargetCount();
            _targetService.ResetTargets();
            for (int column = 0; column < columns; column++)
            {
                var columnQueue = new Queue<TileController>();
                for (int row = 0; row < rows; row++)
                {
                    // Create a new tile controller
                    var tileController = new TileController(tileView, container, 1, _targetService);

                    // Initialize target controllers for the tile
                    Stack<TargetController> targetControllers = new Stack<TargetController>();

                    for (int i = 0; i < 1; i++) // Add one target per tile (adjust as needed)
                    {
                        var targetController = _targetService.CreateTarget(tileController._tileView.transform);
                        targetController.GridColumn = column;

                        // Find the TargetData for this position
                        var targetData = targetTypes.Find(p => p.X == column && p.Y == row);
                        //Debug.LogError(targetData.TargetType);
                        targetController.TargetType = targetData.TargetType; // Assign TargetType
                        targetController.SetColor(targetController.TargetType.Color);
                        targetControllers.Push(targetController);
                    }

                    // Assign target controllers to the tile
                    tileController.SetTargetControllers(targetControllers);

                    // Set the tile's position
                    tileController.SetLocalPosition(new Vector2(column, row));

                    // Enqueue the tile in the column
                    columnQueue.Enqueue(tileController);
                }

                // Add the column to the grid
                _columns.Add(columnQueue);
            }
        }


        public void CleanGrid()
        {
            if (_columns != null)
            {
                for (int column = 0; column < _columnsCount; column++)
                {
                    while (_columns[column].Count > 0)
                    {
                        var tile = _columns[column].Dequeue();
                        GameObject.Destroy(tile._tileView.gameObject);
                    }
                }
            }
        }
        public TileController GetBottomTile(int column)
        {
            if (column < 0 || column >= _columnsCount || _columns[column].Count == 0)
                return null;

            return _columns[column].Peek(); // Return the bottom-most tile
        }

        public void DestroyBottomTile(int column)
        {
            if (column < 0 || column >= _columnsCount || _columns[column].Count == 0)
                return;

            // Get the bottom tile
            var bottomTile = _columns[column].Peek();
            if (bottomTile != null)
            {
                int targetCount = bottomTile.GetTargetCount();

                // If the tile has targets, remove the bottom-most target
                if (targetCount > 0)
                {
                    var target = bottomTile.RemoveTarget();
                    _eventService.OnTargetRemoved?.InvokeEvent(target);

                    // If no targets remain, destroy the tile
                    if (bottomTile.GetTargetCount() == 0)
                    {
                        _columns[column].Dequeue();
                        GameObject.Destroy(bottomTile._tileView.gameObject);
                    }
                }
                else
                {
                    // Dequeue and destroy the tile if it has no targets
                    _columns[column].Dequeue();
                    GameObject.Destroy(bottomTile._tileView.gameObject);
                }
            }

            // Notify about the new bottom tile, if any
            var newBottomTile = GetBottomTile(column);
            if (newBottomTile != null)
            {
                _eventService.OnNewColumnTarget?.InvokeEvent(newBottomTile.GetTopTargetController());
            }
            else
            {
                for (int i = 0; i < _columnsCount; i++)
                {
                    newBottomTile = GetBottomTile(i);
                    if (newBottomTile != null)
                    {
                        _eventService.OnNewColumnTarget?.InvokeEvent(newBottomTile.GetTopTargetController());
                    }
                }
            }

            // Shift tiles visually
            int currentRow = 0;
            foreach (var tile in _columns[column])
            {
                tile.SetLocalPosition(new Vector2(column, currentRow));
                currentRow++;
            }
        }


        public void OnTargetsLoaded()
        {
            List<TargetController> targets = new List<TargetController>();
            for (int column = 0; column < _columnsCount; column++)
            {
                var bottomTile = GetBottomTile(column);
                if (bottomTile != null)
                {
                    targets.Add(bottomTile.GetTopTargetController());
                }
            }
            _eventService.OnTargetLoaded?.InvokeEvent(targets);
        }

    }
}
