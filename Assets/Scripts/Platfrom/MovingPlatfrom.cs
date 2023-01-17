using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatfrom : MonoBehaviour
{
    public bool movePlayer;
    public float moveSpeed = 3f;

    public MovementDirection movementDirection;
    public enum MovementDirection
    {
        Elevator, Sideways
    }
    
    [Header ("Sideways")]
    public float Forwardposition;
    public float Backwardposition;

    [Header("Elevator")]
    public float UpPosition;
    public float DownPosition;

    bool moveRight = true;

    // Update is called once per frame
    void Update()
    {
        if(movementDirection == MovementDirection.Sideways)
        {
            if (transform.position.x >= Forwardposition)
                moveRight = false;
            if (transform.position.x <= Backwardposition)
                moveRight = true;

            if (moveRight)
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            else
                transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }

        if (movementDirection == MovementDirection.Elevator)
        {
            if (transform.position.y >= UpPosition)
                moveRight = false;
            if (transform.position.y <= DownPosition)
                moveRight = true;

            if (moveRight)
                transform.position = new Vector2(transform.position.x , transform.position.y + moveSpeed * Time.deltaTime);
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && movePlayer)
        {
            collision.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && movePlayer)
        {
            collision.transform.parent = null;
        }
    }

    private void OnDisable()
    {
        //if(movePlayer)  gameObject.transform.DetachChildren();
    }
}
