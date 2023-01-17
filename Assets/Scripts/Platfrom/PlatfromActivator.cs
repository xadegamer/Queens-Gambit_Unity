using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromActivator : MonoBehaviour
{
    public GameObject movingPlatfrom;
    public GameObject platfrom;
    public float range;

    public void ActivatePlatfrom()
    {
        float RandomPosition = Random.Range(-range, range);

        platfrom.transform.position = new Vector2(transform.position.x + RandomPosition, transform.position.y);

        platfrom.SetActive(true);
        movingPlatfrom.SetActive(true);
    }

    public void DeactivatePlatfrom()
    {
        platfrom.SetActive(false);
        movingPlatfrom.SetActive(false);
    }
}
