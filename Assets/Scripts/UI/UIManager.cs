using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Gradient healthGradient;
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text pumpinText;
    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        Instance =  this;
    }

    public void SetHealthBar(float newValue)
    {
        healthBar.color = healthGradient.Evaluate(newValue);
        healthBar.fillAmount = newValue;
    }

    public void SetPumpkinText(int currentAmount, int requiredAmount)
    {
        pumpinText.text = currentAmount + " / " + requiredAmount;
    }

    public void SetScroreText(int amount)
    {
        scoreText.text = amount.ToString();
    }
}
