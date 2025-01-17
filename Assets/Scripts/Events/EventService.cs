

using Blaster.Target;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Events
{
    public class EventService
    {
        #region Game Events
        public EventController<int> OnGameStart { get; private set; }
        public EventController<bool> OnGamePause { get; private set; }
        public EventController<bool> OnGameResume { get; private set; }
        public EventController<bool> OnGameEnd { get; private set; }

        public EventController<float> OnUpdateProgress { get; private set; }
        #endregion
        public EventController<List<TargetController>> OnTargetLoaded { get; private set; }
        public EventController<TargetController> OnNewColumnTarget { get; private set; }
        public EventController<TargetController> OnTargetRemoved { get; private set; }
        public EventController<int> OnMapSelected { get; private set; }
        public EventController<int> OnWaveStart { get; private set; }
        public EventController<bool> OnGameOver { get; private set; }
        public EventService()
        {
            OnMapSelected = new EventController<int>();
            OnWaveStart = new EventController<int>();
            OnGameOver = new EventController<bool>();
            OnTargetLoaded = new EventController<List<TargetController>>();
            OnNewColumnTarget = new EventController<TargetController>();
            OnTargetRemoved = new EventController<TargetController>();
            OnGameStart = new EventController<int>();
            OnGamePause = new EventController<bool>();
            OnGameResume = new EventController<bool>();
            OnGameEnd = new EventController<bool>();
            OnUpdateProgress = new EventController<float>();
        }

    }
}