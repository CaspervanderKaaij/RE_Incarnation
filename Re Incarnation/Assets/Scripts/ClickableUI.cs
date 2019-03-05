using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;
    bool onObject = false;
    [SerializeField] string inputName = "Attack_P1";
    [SerializeField] UnityEvent clickEvent;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        onObject = true;
        enterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        onObject = false;
        exitEvent.Invoke();
    }

    void Update()
    {
        if (onObject == true && Input.GetButtonDown(inputName) == true)
        {
            clickEvent.Invoke();
        }
    }
}
