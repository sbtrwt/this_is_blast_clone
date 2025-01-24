
using Blaster.Events;
using Blaster.Level;
using Blaster.Target;
using System.Collections;
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
        private GameService _gameService;
        public GridService(int rows, int columns, TileView tileView, Transform container)
        {
            _rows = rows;
            _columnsCount = columns;

            _tileView = tileView;
            _container = container;
        }
        public void Init(EventService eventService, TargetService targetService, GameService gameService)
        {
            this._eventService = eventService;
            this._targetService = targetService;
            this._gameService = gameService;
            //CreateGrid(_rows, _columnsCount, _tileView, _container);
            //OnTargetsLoaded();
        }
        public void CreateGrid(int rows, int columns, TileView tileView, Transform container, List<TargetData> targetTypes, int height = 1)
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
                    var tileController = new TileController(tileView, container, height, _targetService);

                    // Initialize target controllers for the tile
                    Stack<TargetController> targetControllers = new Stack<TargetController>();

                    for (int i = 0; i < height; i++) // Add one target per tile (adjust as needed)
                    {
                        var targetController = _targetService.CreateTarget(tileController._tileView.transform);
                        targetController.GridColumn = column;

                        // Find the TargetData for this position
                        var targetData = targetTypes.Find(p => p.X == column && p.Y == row);
                        //Debug.LogError(targetData.TargetType);
                        targetController.TargetType = targetData.TargetType; // Assign TargetType
                        targetController.SetColor(targetController.TargetType.Color);
                       
                        // Calculate position for stacking with 3D perspective
                        float zOffset = -i * 0.5f; // Simulate depth by decreasing Z as we go up
                        float yOffset = i * 0.2f;  // Adjust vertical spacing as needed

                        // Adjust the position of the target
                        targetController.SetLocalPosition( new Vector3(0, yOffset, zOffset));

                        // Optional: Adjust scale to enhance 3D effect
                        float scaleFactor = 1.0f - (i * 0.05f); // Slightly reduce scale as it stacks higher
                        targetController.SetLocalScale( new Vector3(scaleFactor, scaleFactor, 1));

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
            //int currentRow = 0;
            //foreach (var tile in _columns[column])
            //{
            //    tile.SetLocalPosition(new Vector2(column, currentRow));
            //    currentRow++;
            //}
            _gameService.StartCoroutine(ShiftTilesWithAnimation(column));
        }
        private IEnumerator ShiftTilesWithAnimation(int column)
        {
            int currentRow = 0;
            foreach (var tile in _columns[column])
            {
                Vector2 targetPosition = new Vector2(column, currentRow);
                _gameService. StartCoroutine(SmoothMove(tile._tileView.transform, targetPosition, 0.3f)); // Adjust duration as needed
                currentRow++;
            }
            yield return null;
        }

        private IEnumerator SmoothMove(Transform transform, Vector2 targetPosition, float duration)
        {
            Vector2 startPosition = transform.localPosition;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                if (transform != null)
                    transform.localPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if(transform != null)
            transform.localPosition = targetPosition; // Ensure the final position is set
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
