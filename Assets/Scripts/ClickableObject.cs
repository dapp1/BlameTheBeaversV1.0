
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    public UnityEvent ClickEvent;
    public int ClickOrderPriority;

    public void OnClick()
    {
        ClickEvent?.Invoke();
    }
}
