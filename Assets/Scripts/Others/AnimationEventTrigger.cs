using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent[] eventsToTrigger;

    public void TriggerEvent(int eventIndex)
    {
        eventsToTrigger[eventIndex].Invoke();
    }
}
