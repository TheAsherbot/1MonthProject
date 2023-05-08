using System;

using UnityEngine;

public class HealthSystem
{


    public event EventHandler OnHealthDepleted;
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public int value;
        public int amount;
    }



    private int maxHealth;
    private int health;


    public HealthSystem(int maxHealth) 
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercent()
    {
        Debug.Log("(float)health / maxHealth: " + (float)health / maxHealth);
        return (float)health / maxHealth;
    }

    public void Damage(int damageAmount)
    {
        Debug.Log("damageAmount: " + damageAmount);
        Debug.Log("Old health: " + health);
        health -= damageAmount;
        Debug.Log("health: " + health);
        Debug.Log("(float)health / maxHealth: " + (float)health / maxHealth);
        
        if (health <= 0)
        {
            Debug.Log("health <= 0");
            health = 0;
            OnHealthDepleted?.Invoke(this, EventArgs.Empty);
            return;
        }
        
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
        {
            value = health,
            amount = -damageAmount,
        });
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        
        if (health > maxHealth)
        {
            health = maxHealth;
            return;
        }
        
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
        {
            value = health,
            amount = healAmount,
        });
    }

}
