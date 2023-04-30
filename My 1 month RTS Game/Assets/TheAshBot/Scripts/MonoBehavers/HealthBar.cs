using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    /// <summary>
    /// This Heals the object with this script
    /// </summary>
    /// <param name="healAmount">This is the amount that heal reganded</param>
    public void Heal(int healAmount)
    {
        SetHealth(healAmount);
    }

    /// <summary>
    /// This Dameges the object with this script
    /// </summary>
    /// <param name="damegeAmount">This is the amount of damege delt</param>
    public void Damege(int damegeAmount)
    {
        SetHealth(-damegeAmount);
    }

    /// <summary>
    /// This sets the max health amount
    /// </summary>
    /// <param name="health">This is that max health amount</param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>
    /// This checks if the object with this script is dead
    /// </summary>
    /// <returns>Returns true if the object with this script is dead if not then returns fales</returns>
    public bool IsDead()
    {
        if (slider.value <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    /// <summary>
    /// This sets the health amount to the health pased in
    /// </summary>
    /// <param name="health">This is what the health will be set to</param>
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
