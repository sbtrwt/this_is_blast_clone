using Blaster.Events;
using Blaster.Target;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
            CreateGrid(_rows, _columnsCount, _tileView, _container);
            OnTargetsLoaded();
        }
        private void CreateGrid(int rows, int columns, TileView tileView, Transform container)
        {
            _columns = new List<Queue<TileController>>();

            for (int column = 0; column < columns; column++)
            {
                var columnQueue = new Queue<TileController>();
                for (int row = 0; row < rows; row++)
                {
                   
                    var tileController = new TileController(tileView, container, 1 , _targetService);
                    Stack<TargetController> targetControllers = new Stack<TargetController>();
                    for (int i = 0; i < 1; i++)
                    {
                        var targetController = _targetService.CreateTarget(tileController._tileView.transform);
                        targetController.GridColumn = column;
                        targetControllers.Push(targetController);
                    }
                    tileController.SetTargetControllers(targetControllers);
                    tileController.SetLocalPosition(new Vector2(column, row));
                    columnQueue.Enqueue(tileController);
                }
                _columns.Add(columnQueue);
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
