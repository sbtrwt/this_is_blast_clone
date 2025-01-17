using Blaster.Events;
using Blaster.Grid;
using Blaster.Sound;
using Blaster.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blaster.Target
{
    public class TargetService
    {
        private GridService _gridService;
        private EventService _eventService;
        private SoundService _soundService;
        private List<TargetController> targets = new List<TargetController>();
        private TargetSO _targetSO;
        private int _targetCount = 0;
        private ParticleSystem _smokeParticle;
        public TargetService(TargetSO targetSO, ParticleSystem smokeParticle)
        {
            _targetSO = targetSO;
            _smokeParticle = smokeParticle;
        }
        public void Init(GridService gridService, EventService eventService, SoundService soundService)
        {
            this._gridService = gridService;
            this._eventService = eventService;
            this._soundService = soundService;
        }
        public TargetController CreateTarget(Transform container)
        {
            TargetController targetController = new TargetController(_targetSO, this, container);
            targets.Add(targetController);
            _targetCount++;
            return targetController;
        }
        public void RemoveTarget(TargetController targetController)
        {
            ShowSmokeEffect(targetController.GetTransform().position);
            _soundService.PlaySoundEffects(SoundType.TargetPop);
            targets.Remove(targetController);
            _gridService.DestroyBottomTile(targetController.GridColumn);
            Debug.Log("target count:    " + targets.Count);
            Debug.Log("target count:    " + _targetCount);
            _eventService.OnUpdateProgress.InvokeEvent((float)targets.Count/_targetCount);
            if(targets.Count == 0)
            {
                _eventService.OnGameEnd.InvokeEvent(true);
            }
        }
        public void ShowSmokeEffect(Vector3 position)
        {
            var tempSmoke = GameObject.Instantiate(_smokeParticle);
            position.z = -3;
            tempSmoke.transform.position = position;
            tempSmoke.Play();
            GameObject.Destroy(tempSmoke.gameObject, 1f);
        }
    }
}
