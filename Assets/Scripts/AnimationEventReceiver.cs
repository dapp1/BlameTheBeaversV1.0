using System;
using UnityEngine;

namespace AnimationHelper
{
    public enum AnimationEventType
    {
        Hit, Death
    }
    
    public class AnimationEventReceiver : MonoBehaviour
    {
        public Action OnEvent;

        public void OnAnimationEvent(string eventName)
        {
            if (Enum.TryParse(eventName, out AnimationEventType type))
            {
                OnEvent?.Invoke();
            }
        }
    }
}