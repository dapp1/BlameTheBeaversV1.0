using System;
using System.Collections.Generic;

namespace EventBusSystem
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var existing))
                _events[type] = Delegate.Combine(existing, callback);
            else
                _events[type] = callback;
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var existing))
            {
                var newDelegate = Delegate.Remove(existing, callback);

                if (newDelegate == null)
                    _events.Remove(type);
                else
                    _events[type] = newDelegate;
            }
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var del))
            {
                (del as Action<T>)?.Invoke(eventData);
            }
        }
    }
}