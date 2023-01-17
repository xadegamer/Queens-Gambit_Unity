using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float movespeed;
    [SerializeField] float distanceCheck;
    [SerializeField] Vector2 upPos;
    [SerializeField] Vector2 downPos;

    Vector2 direction;
    [SerializeField] float distanceToPoint;

    private void Update()
    {
        MoveDoor();
    }

    public void Open()
    {
        direction = Vector2.right;
    }

    public void Close()
    {
        direction = Vector2.left;
    }

    void MoveDoor()
    {
        if(direction == Vector2.right)
            distanceToPoint = Vector2.Distance(transform.position, upPos);
        else
            distanceToPoint = Vector2.Distance(transform.position, downPos);

        if (distanceToPoint > distanceCheck)
            transform.Translate(direction * movespeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        upPos.x = transform.position.x;
        downPos.x = transform.position.x;
        Gizmos.DrawWireSphere(upPos, .2f);
        Gizmos.DrawWireSphere(downPos, .2f);
    }

    public void Check(string message)
    {
        Debug.Log(message);
    }
}
