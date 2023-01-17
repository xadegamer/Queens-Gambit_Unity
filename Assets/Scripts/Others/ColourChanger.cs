using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColourChanger : MonoBehaviour
{
    [SerializeField] float timer;
    bool isChanged = false;
    private float timeLimit;
    TextMeshProUGUI textDisplay;
    void Start()
    {
        textDisplay=  GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if ( timeLimit <= 0)
        {
            textDisplay.color = isChanged ? Color.red : Color.yellow;
            isChanged = !isChanged;
            timeLimit = timer;
        }
        else timeLimit -= Time.deltaTime;
    }
}
