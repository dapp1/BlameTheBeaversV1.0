
using System;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    public UnityEvent ClickEvent;
    public int ClickOrderPriority;

    private void OnMouseDown()
    {
        OnClick();
    }

    public void OnClick()
    {
        ClickEvent?.Invoke();
    }
}
