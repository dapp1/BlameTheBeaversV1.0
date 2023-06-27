using System;

namespace Nora.NEvent
{
    public class NSubscription
    {
        public NHandler Handler;
        public Type Type;
    }

    public class NSubscription<T> : NSubscription
    {
        public new NHandler<T> Handler;
    }

    public delegate void NHandler<T>(NEvent<T> data);
    public delegate void NHandler();

    public enum EventStatus
    {
        Created,
        Accepted,
        Busy,
        Cancelled,
        Ended,
    }
}


