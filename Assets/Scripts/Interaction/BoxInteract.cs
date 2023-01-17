using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxInteract : MonoBehaviour
{
    [SerializeField] UnityEvent OnOpen;

    bool isOpen = false;
    public void BoxOpen()
    {
        if (isOpen) return;

        isOpen = true;
        OnOpen.Invoke();
    }
}
