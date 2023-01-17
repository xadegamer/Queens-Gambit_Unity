using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AdvancedPlatform : MonoBehaviour
{
    [Header("Properties")]
    public float speed;
    public Vector2[] wayPoints;

    [Header("Settings")]
    public bool movePlayer;
    public bool loop;

    public Vector2 startPos;
    private Vector2 currentWayPoint;
    private int wayPointIndex = -1;
    bool reverse;

    void Start()
    {
        startPos = transform.position;
        currentWayPoint = GetMidPos(wayPoints[0]);
    }

    void Update()
    {
        MoveToWayPoint();
        CheckDistanceToWayPoint();
    }

    public void MoveToWayPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
    }

    public void CheckDistanceToWayPoint()
    {
        float distance = Vector2.Distance(transform.position, currentWayPoint);

        if (distance == 0f)
        {
            if (loop)
            {
                if (wayPointIndex == wayPoints.Length - 1)
                {
                    wayPointIndex = -1;
                    currentWayPoint = GetMidPos(startPos);
                }
                else
                {
                    wayPointIndex++;
                    currentWayPoint = GetMidPos(wayPoints[wayPointIndex]);
                }
            }
            else
            {
                if (wayPointIndex == 0)
                {
                    if (reverse)
                    {
                        wayPointIndex = -1;
                        currentWayPoint = GetMidPos(startPos);
                    }
                    else
                    {
                        wayPointIndex++;
                        currentWayPoint = GetMidPos(wayPoints[wayPointIndex]);
                    }

                    reverse = false;
                }
                else
                {
                    if (wayPointIndex == wayPoints.Length - 1)
                    {
                        reverse = true;
                        wayPointIndex--;
                    }
                    else
                    {
                        if (reverse)
                        {
                            wayPointIndex--;
                        }
                        else
                        {
                            wayPointIndex++;
                        }
                    }

                    currentWayPoint = GetMidPos(wayPoints[wayPointIndex]);
                }
            }
        }
    }


    public Vector2 GetMidPos(Vector2 point)
    {
       Vector2 newPos =  new Vector2(point.x, point.y);
       newPos.x -= transform.GetComponent<Collider2D>().bounds.extents.x;
       return newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && movePlayer)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(startPos, .2f);
        Gizmos.DrawLine(startPos, wayPoints[0]);

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(wayPoints[i], .2f);
        }

        Vector2 startPos1 = wayPoints[0];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawLine(startPos1, wayPoints[i]);
            startPos1 = wayPoints[i];
        }

        if (loop) Gizmos.DrawLine(startPos, wayPoints[wayPoints.Length - 1]);
    }
}
