using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBoard : MonoBehaviour
{
    [SerializeField] TMP_Text messageDisplay;
    [SerializeField] Message[] messages;
    [SerializeField] float messageTime;

    [Header("Debug")]
    [SerializeField] float timer;
    [SerializeField] int currentMessage;

    private void Update()
    {
        if (timer < 0)
        {
            DisplayMessages();
            timer = messageTime;
        }
        else timer -= Time.deltaTime;
    }

    public void DisplayMessages()
    {
        messageDisplay.text = messages[currentMessage].info;
        currentMessage++;

        if (currentMessage == messages.Length) currentMessage = 0;
    }
}
