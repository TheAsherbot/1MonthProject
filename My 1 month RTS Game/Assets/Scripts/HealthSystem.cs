using System;

public class HealthSystem
{


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

    public float GetHealthPercent()
    {
        return (float)health / maxHealth;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        
        if (health < 0)
        {
            health = 0;
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
