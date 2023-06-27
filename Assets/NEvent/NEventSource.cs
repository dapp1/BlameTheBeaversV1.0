using System;
using UnityEngine;

namespace Nora.NEvent
{
    public class NEventSource : MonoBehaviour
    {
        public void StartEvent<T>(T msg, Action<EventStatus> onStatusChanged = null)
        {
            NEventManager.StartEvent(msg, this, onStatusChanged);
        }
    }
}

