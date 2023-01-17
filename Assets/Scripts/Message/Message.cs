using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Message", menuName = "Game/Message", order = 1)]
public class Message : ScriptableObject
{
    [TextArea(15, 20)]
    public string info;
}
