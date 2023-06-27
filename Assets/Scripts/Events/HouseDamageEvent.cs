using Nora.NEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    internal class HouseDamageEvent : NEvent<int>
    {
        public HouseDamageEvent(int msg, NEventSource source = null, Action<EventStatus> onStatusChanged = null) : base(msg, source, onStatusChanged)
        {
        }
    }
}
