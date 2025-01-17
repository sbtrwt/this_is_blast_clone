using Blaster.Events;
using Blaster.Grid;
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
        private List<TargetController> targets = new List<TargetController>();
        private TargetSO _targetSO;
        private int _targetCount = 0;
        public TargetService(TargetSO targetSO)
        {
            _targetSO = targetSO;
        }
        public void Init(GridService gridService, EventService eventService)
        {
            this._gridService = gridService;
            this._eventService = eventService;
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
            targets.Remove(targetController);
            _gridService.DestroyBottomTile(targetController.GridColumn);
            Debug.Log("target count:    " + targets.Count);
            Debug.Log("target count:    " + _targetCount);
            _eventService.OnUpdateProgress.InvokeEvent((float)targets.Count/_targetCount);
        }
    }
}
