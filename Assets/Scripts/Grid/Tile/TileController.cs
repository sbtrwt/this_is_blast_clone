using Blaster.Target;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Grid
{
    public class TileController
    {
        public TileView _tileView { get; private set; }
        private Transform _container;
        private int targetCount;
        private Stack<TargetController> _targetControllers;
        private TargetService _targetService;
        public TileController(TileView tileView, Transform container, int targetCount,  TargetService targetService)
        {
            _tileView = GameObject.Instantiate(tileView, container);
            _tileView.Controller = this;
            _container = container;
            this.targetCount = targetCount;
          
            _targetService = targetService;
        }
        public void SetTargetControllers(Stack<TargetController> targetControllers)
        {
            _targetControllers = targetControllers;
        }
        public void SetPosition(Vector2 position)
        {
            _tileView.transform.position = new Vector3(position.x, position.y, 0);
        }

        public Vector2 GetPosition()
        {
            return _tileView.transform.position;
        }
        public Transform GetTransform()
        {
            return _tileView.transform;
        }
        public Transform GetTopTargetTransform()
        {
            if (_targetControllers.Count == 0)
            {
                return null;
            }
            _targetControllers.Peek().IsActive = true;
            return _targetControllers.Peek().GetTransform();
        }
        public TargetController RemoveTarget()
        {
            TargetController targetController = _targetControllers.Pop();
            //_targetService.RemoveTarget(targetController);
            return targetController;
        }
        public int GetTargetCount()
        {
            return _targetControllers.Count;
        }
    }

}
