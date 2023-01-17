using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractOnButton : InteractOnTrigger
{
    public KeyCode button;
    public UnityEvent OnButtonPress;

    bool canExecuteButtons = false;

    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);
        canExecuteButtons = true;
    }

    protected override void ExecuteOnExit(Collider2D other)
    {
        base.ExecuteOnExit(other);
        canExecuteButtons = false;
    }

    void Update()
    {
        if (canExecuteButtons && Input.GetKeyDown(button))
        {
            OnButtonPress.Invoke();
        }
    }
}
