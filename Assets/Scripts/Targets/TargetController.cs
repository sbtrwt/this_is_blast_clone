using Assets.Scripts.Targets;
using Blaster.Targets;
using UnityEngine;

namespace Blaster.Target
{
    public class TargetController
    {
        protected TargetView _targetView;
        //private BlockSO blockSO;
        private float _health;
        private TargetService _targetService;
        private int _gridColumn;
        private bool _isActive = false;
        private bool _isLocked = false;
        private TargetType _targetType;
        private TargetSO _targetSO;
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public int GridColumn { get => _gridColumn; set => _gridColumn = value; }
        public TargetType TargetType { get => _targetSO.TargetType; }
        public bool IsLocked { get => _isLocked; set => _isLocked = value; }

        public TargetController(TargetSO targetSO, TargetService targetService, Transform container)
        {
            _targetSO = targetSO;
            this._targetView = GameObject.Instantiate(targetSO.TargetPrefab, container);
            this._targetView.Controller = this; 
            _health = 1;
            this._targetService = targetService;
        }

        public virtual void TakeDamage(float damage)
        {
            if(!_isActive) { return; }
            _health -= damage;
            Debug.Log("Target attacked");
            if (_health <= 0)
            {
                Debug.Log("Target destroyed");
                DestroyBlock();
                _targetService.RemoveTarget(this);
            }
        }

        private void DestroyBlock()
        {
            UnityEngine.Object.Destroy(_targetView.gameObject);
        }
        public Transform GetTransform()
        {
            if (_targetView == null) { return null; }
            return _targetView.transform;
        }
    }
}
