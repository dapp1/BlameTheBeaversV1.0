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

    public class OnClickItemEvent
    {
        public InventoryItemObject Item;
        public Action OnPick;
        
        public OnClickItemEvent(InventoryItemObject item, Action onPick)
        {
            Item = item;
            OnPick = onPick;
        }
    }
    
    public class OnActiveItemChangedEvent
    {
        public InventoryItemDto Item;

        public OnActiveItemChangedEvent(InventoryItemDto item)
        {
            Item = item;
        }
    }
}