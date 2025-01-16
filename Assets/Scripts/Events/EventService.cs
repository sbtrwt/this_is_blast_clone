

using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Events
{
    public class EventService
    {
        public EventController<List<Transform>> OnTargetLoaded { get; private set; }
        public EventController<Transform> OnNewColumnTarget { get; private set; }
        public EventController<Transform> OnTargetRemoved { get; private set; }
        public EventController<int> OnMapSelected { get; private set; }
        public EventController<int> OnWaveStart { get; private set; }
        public EventController<bool> OnGameOver { get; private set; }
        public EventService()
        {
            OnMapSelected = new EventController<int>();
            OnWaveStart = new EventController<int>();
            OnGameOver = new EventController<bool>();
            OnTargetLoaded = new EventController<List<Transform>>();
            OnNewColumnTarget = new EventController<Transform>();
            OnTargetRemoved = new EventController<Transform>();
        }

    }
}