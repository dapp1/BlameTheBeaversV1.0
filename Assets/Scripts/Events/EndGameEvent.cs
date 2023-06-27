using Nora.NEvent;
using System;

namespace Assets.Scripts.Events
{
    internal class EndGameEvent : NEvent<EndGameReason>
    {
        public EndGameEvent(EndGameReason msg, NEventSource source = null, Action<EventStatus> onStatusChanged = null) : base(msg, source, onStatusChanged)
        {
        }
    }
}
