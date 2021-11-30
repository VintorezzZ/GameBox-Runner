using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonDownClick : MonoBehaviour, IPointerDownHandler
{

    public UnityEvent OnButtonDownClick;
    private bool pointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    private void Update()
    {
        if (pointerDown)
        {
            if (OnButtonDownClick != null)
                OnButtonDownClick.Invoke();
            
        }
    }
}
