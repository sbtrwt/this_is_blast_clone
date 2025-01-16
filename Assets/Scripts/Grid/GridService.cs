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
                    var tileController = new TileController(tileView, container);
                    var targetController = _targetService.CreateTarget(tileController._tileView.transform);
                    targetController.GridColumn = column;
                    tileController.SetPosition(new Vector2(column, row));
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

            // Remove the bottom tile
            var bottomTile = _columns[column].Dequeue();
            _eventService.OnTargetRemoved.InvokeEvent(bottomTile.GetTransform());
            if (bottomTile != null)
            {
                GameObject.Destroy(bottomTile._tileView.gameObject);
            }
            if (GetBottomTile(column) != null)
            {
                _eventService.OnNewColumnTarget.InvokeEvent(GetBottomTile(column)._tileView.transform);
            }// Shift all tiles visually
            int currentRow = 0;
            foreach (var tile in _columns[column])
            {
                tile.SetPosition(new Vector2(column, currentRow));
                currentRow++;
            }
        }

        public void OnTargetsLoaded()
        {
            List<Transform> targets = new List<Transform>();
            for(int column = 0; column < _columnsCount; column++)
            {
                targets.Add(GetBottomTile(column)._tileView.transform);
            }
            _eventService.OnTargetLoaded.InvokeEvent(targets);
        }
    }
}
