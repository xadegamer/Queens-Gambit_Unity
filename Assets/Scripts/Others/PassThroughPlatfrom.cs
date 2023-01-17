using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatfrom : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] bool onPlatfrom;
    [SerializeField] bool canChange;

    private PlatformEffector2D platformEffector;

    WaitForSeconds delayTime;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        delayTime = new WaitForSeconds(delay);
    }

    IEnumerator GoDown()
    {
        platformEffector.rotationalOffset = 180;
        yield return delayTime;
        platformEffector.rotationalOffset = 0;
        canChange = true;
    }

    private void Update()
    {
        if (onPlatfrom && Input.GetAxisRaw("Vertical") == -1) // Check if the player is presing the up or down button
        {
            if (canChange)
            {
                canChange = false;
                StartCoroutine(nameof(GoDown));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onPlatfrom = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlatfrom = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetAxisRaw("Vertical") == -1) // Check if the player is presing the up or down button
            {
                if (canChange)
                {
                    canChange = false;
                    StartCoroutine(nameof(GoDown));
                }
            }
        }
    }
}
