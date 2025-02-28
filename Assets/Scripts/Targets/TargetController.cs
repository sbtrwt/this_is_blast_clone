﻿using Blaster.Grid;
using Blaster.Targets;
using Blaster.Weapon;
using System.Collections;
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
        public TargetType TargetType { get => _targetType; set => _targetType = value; }
        public bool IsLocked { get => _isLocked; set => _isLocked = value; }
        public WeaponController TargetLockedBy { get; set; }
        public bool IsTargetGotHit { get; set; }
        public int TargetHitCounter { get; set; }
        public TargetController(TargetSO targetSO, TargetService targetService, Transform container)
        {
            _targetSO = targetSO;
            this._targetView = GameObject.Instantiate(targetSO.TargetPrefab, container);
            this._targetView.Controller = this;
            _health = _targetSO.Health;
            this._targetService = targetService;
            _targetType = _targetSO.TargetType;
            SetColor(_targetSO.TargetType.Color );
        }

        public virtual void TakeDamage(float damage)
        {
            if (!_isActive) { return; }
            _health -= damage;
            IsTargetGotHit = true;
            Debug.Log("Target attacked");
            if (_health <= 0)
            {
                Debug.Log("Target destroyed");
                //_targetView.PlaySmokeParticle();
                _targetView.PlayDestroyAnimation();
                //_targetView.Invoke(nameof( DestroyBlock),0.5f);
                _targetView.StartCoroutine(WaitAndExecuteUnscaled(0.05f));
            }
        }
        private IEnumerator WaitAndExecuteUnscaled(float waitTime)
        {
            Debug.Log("Waiting...");
            yield return new WaitForSecondsRealtime(waitTime);
            DestroyBlock();
        }
        private void DestroyBlock()
        {
            _targetService.RemoveTarget(this);
            UnityEngine.Object.Destroy(_targetView.gameObject);
        }
        public Transform GetTransform()
        {
            if (_targetView == null) { return null; }
            return _targetView.transform;
        }
        public void SetColor(Color color)
        {
            _targetView.Setcolor(color);
        }
        public void SetLocalPosition(Vector3 position)
        {
            _targetView.transform.localPosition = position;
        }
        public void SetLocalScale(Vector3 scale)
        {
            _targetView.transform.localScale = scale;
        }
    }
}
