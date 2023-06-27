using System;

namespace Nora.NEvent
{
    public class NEvent<T>
    {
        public EventStatus Status;
        public Action<EventStatus> OnStatusChanged;
        public T Data;
        public NEventSource Source;

        public NEvent(T msg, NEventSource source = null, Action<EventStatus> onStatusChanged = null)
        {
            Status = EventStatus.Created;
            OnStatusChanged = onStatusChanged;
            Data = msg; 
            Source = source;
        }

        public void Accept()
        {
            Status = EventStatus.Accepted;
            OnStatusChanged?.Invoke(Status);
        }

        public void Cancel()
        {
            Status = EventStatus.Cancelled;
            OnStatusChanged?.Invoke(Status);
        }

        public void End()
        {
            Status = EventStatus.Ended;
            OnStatusChanged?.Invoke(Status);
        }

        public void Busy()
        {
            Status = EventStatus.Busy;
            OnStatusChanged?.Invoke(Status);
        }
    }
}