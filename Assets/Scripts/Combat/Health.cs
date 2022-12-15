using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDeath;

    [SerializeField] int maxHealth = 100;

    int currentHealth;

    public bool IsDead => currentHealth == 0;
    bool isInvulnerable;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        if (currentHealth == 0)
            return;

        if (isInvulnerable)
            return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        OnTakeDamage?.Invoke();

        if (currentHealth == 0)
            OnDeath?.Invoke();
    }
}
