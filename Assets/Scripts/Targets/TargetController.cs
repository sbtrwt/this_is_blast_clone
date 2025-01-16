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

        public int GridColumn { get => _gridColumn; set => _gridColumn = value; }
        public TargetController(TargetView target, TargetService targetService, Transform container)
        {

            this._targetView = GameObject.Instantiate(target, container);
            this._targetView.Controller = this; 
            _health = 1;
            this._targetService = targetService;
        }

        public virtual void TakeDamage(float damage)
        {
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
    }
}
