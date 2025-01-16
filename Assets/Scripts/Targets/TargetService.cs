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
        private List<TargetController> targets = new List<TargetController>();
        private TargetSO _targetSO;
        public TargetService(TargetSO targetSO)
        {
            _targetSO = targetSO;
        }
        public void Init(GridService gridService)
        {
            this._gridService = gridService;
        }
        public TargetController CreateTarget(Transform container)
        {
            TargetController targetController = new TargetController(_targetSO, this, container);
            targets.Add(targetController);
            return targetController;
        }
        public void RemoveTarget(TargetController targetController)
        {
            targets.Remove(targetController);
            _gridService.DestroyBottomTile(targetController.GridColumn);
        }
    }
}
