using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDelay : MonoBehaviour
{
    [SerializeField] float time;

    private void OnEnable()
    {
        Destroy(gameObject, time);
    }
}
