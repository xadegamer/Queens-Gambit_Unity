using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance { get; private set; }

    public Action OnFaded;

    [SerializeField] Animator fadeAnimator;
    [SerializeField] Image fadeSprite;


    private void Awake()
    {
        Instance = this;
    }

    public void ScreenFade(Color fadeColor, Action fadedAction)
    {
        OnFaded = fadedAction;
        fadeSprite.color = fadeColor;
        fadeAnimator.SetTrigger("Fade Screen");
    }

    public void TriggerEvent()
    {
        OnFaded?.Invoke();
    }
}
