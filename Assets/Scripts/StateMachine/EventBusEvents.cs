using System;
using UnityEngine;

namespace EventBusSystem
{
    public class EventBusEvents
    {
        
    }

    public class OnClickRootEvent
    {
        public RootController Root;
        
        public OnClickRootEvent(RootController root)
        {
            Root = root;
        }
    }
    
    public class OnClickBeaverEvent
    {
        public BeaverController Beaver;
        public Action OnKick;
        
        public OnClickBeaverEvent(BeaverController beaver, Action onKick)
        {
            Beaver = beaver;
            OnKick = onKick;
        }
    }
}