using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Gradient healthGradient;
    [SerializeField] GameObject healthBarUI;
    [SerializeField] Image healthBar;
    [SerializeField] int scoreAmount;

    private Animator animator;
    private Damageable damageable;
    private bool isDead;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
        damageable.OnHitEvent().AddListener(DamageTaken);
        damageable.OnDeadEvent().AddListener(Died);
    }

    public void DamageTaken()
    {
        healthBar.color = healthGradient.Evaluate(damageable.HealthPercentage());
        healthBar.fillAmount = damageable.HealthPercentage();
        animator.SetTrigger("Hit");
    }

    public void Died()
    {
        isDead = true;
        healthBarUI.SetActive(false);
        animator.SetTrigger("Die");

        DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        GameManager.Instance.IncrementScore(scoreAmount);
        Destroy(gameObject,3);
    }

    public bool ISDead() => isDead;
}
