using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger : MonoBehaviour
{
    public LayerMask layers;
    public UnityEvent OnEnter, OnExit;
    Collider2D col;

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (0 != (layers.value & 1 << collision.gameObject.layer))
        {
            ExecuteOnEnter(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (0 != (layers.value & 1 << collision.gameObject.layer))
        {
            ExecuteOnExit(collision);
        }
    }

    protected virtual void ExecuteOnEnter(Collider2D other)
    {
        OnEnter.Invoke();
    }


    protected virtual void ExecuteOnExit(Collider2D other)
    {
        OnExit.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position +  new Vector3 (0,10,0), "InteractionTrigger", true);
    }
}
