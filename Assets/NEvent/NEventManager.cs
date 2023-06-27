using System.Collections.Generic;
using System;
using Pixelplacement;

namespace Nora.NEvent
{
    public class NEventManager : Singleton<NEventManager>
    {
        public Dictionary<Type,List<NSubscription>>  Subscriptions = new Dictionary<Type, List<NSubscription>>();

        public static NSubscription<T> Subscribe<T>(NHandler<T> handler) where T : class
        {
            var subscription = new NSubscription<T> {
                Handler = handler,
                Type = typeof(T)
            };

            Subscribe(subscription);

            return subscription;
        }

        public static NSubscription Subscribe<T>(NHandler handler) where T : class
        {
            var subscription = new NSubscription
            {
                Handler = handler,
                Type = typeof(T)
            };

            Subscribe(subscription);

            return subscription;
        }

        private static void Subscribe(NSubscription subscription)
        {
            if (!Instance.Subscriptions.ContainsKey(subscription.Type))
                Instance.Subscriptions.Add(subscription.Type, new List<NSubscription>());

            Instance.Subscriptions[subscription.Type].Add(subscription);
        }

        public static void StartEvent<T>(T msg, NEventSource source = null, Action<EventStatus> onStatusChanged = null)
        {
            Type type = msg.GetType();

            if (!Instance.Subscriptions.ContainsKey(type))
                return;

            var e = new NEvent<T>(msg, source, onStatusChanged);
 
            foreach (var subscription in Instance.Subscriptions[type])
            {
                var handler = (NSubscription<T>)subscription;
                handler.Handler.Invoke(e);
            } 
        }

        public static void Clear()
        {
            Instance.Subscriptions.Clear();
        }
    }
}
