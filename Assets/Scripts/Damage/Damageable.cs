using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float damageCoolDown;

    [SerializeField] UnityEvent OnHit;
    [SerializeField] UnityEvent OnHeal;
    [SerializeField] UnityEvent OnDead;

    private WaitForSeconds cooldownTime;
    private bool isInvulnerable = false;
    private int health;

    private void Start()
    {
        health = maxHealth;
        cooldownTime = new WaitForSeconds(damageCoolDown);
    }
    public void SetUp(int healthValue)
    {
        health = maxHealth = healthValue;
    }

    public void TakeDamage(int value)
    {
        if (health <= 0 || isInvulnerable) return;
        
        health -= value;
        OnHit.Invoke();

        StartCoroutine(nameof(StartCooldown));

        if (health <= 0)
        {
            health = 0;
            OnDead.Invoke();
        }
    }

    public void Heal(int value)
    {
        health += value;
        if (health > maxHealth) health = maxHealth;
        OnHeal.Invoke();
    }

    public IEnumerator StartCooldown()
    {
        isInvulnerable = true;
        yield return  cooldownTime;
        isInvulnerable = false;
    }

    public bool HealthFull() => health == maxHealth;
    public float HealthPercentage() => (float)health / (float) maxHealth;
    public void ToggleInvulnerabuility(bool newState) => isInvulnerable = newState;

    public UnityEvent OnHitEvent() => OnHit;
    public UnityEvent OnHealEvent() => OnHeal;
    public UnityEvent OnDeadEvent() => OnDead;
}
