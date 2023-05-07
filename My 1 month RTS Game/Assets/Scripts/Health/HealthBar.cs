using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public class Border
    {
        public float thickness;
        public Color color;
    }


    public static HealthSystem Create(int maxHealth, Transform fallow, Vector3 offset, Vector3 size, Color barColor, Color backgroundColor, Border border = null, bool hideWhenFull = false)
    {
        // Main Health Bar
        GameObject healthBarGameObject = new GameObject("HealthBar");
        healthBarGameObject.transform.localPosition = offset;

        // Placeholder
        GameObject contentGameObject = new GameObject("Content");

        if (border != null)
        {
            // Border
            GameObject borderGameObject = new GameObject("Border", typeof(SpriteRenderer));
            borderGameObject.transform.SetParent(healthBarGameObject.transform);
            borderGameObject.transform.localPosition = Vector3.zero;
            borderGameObject.transform.localScale = size + Vector3.one * border.thickness;
            borderGameObject.GetComponent<SpriteRenderer>().color = border.color;
            borderGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.singlePixelSprite;
            borderGameObject.GetComponent<SpriteRenderer>().sortingOrder = 90;
        }

        // Background
        GameObject backgroundGameObject = new GameObject("Background", typeof(SpriteRenderer));
        backgroundGameObject.transform.SetParent(healthBarGameObject.transform);
        backgroundGameObject.transform.localPosition = Vector3.zero;
        backgroundGameObject.transform.localScale = size;
        backgroundGameObject.GetComponent<SpriteRenderer>().color = backgroundColor;
        backgroundGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.singlePixelSprite;
        backgroundGameObject.GetComponent<SpriteRenderer>().sortingOrder = 100;

        // Bar
        GameObject barGameObject = new GameObject("Bar");
        barGameObject.transform.SetParent(healthBarGameObject.transform);
        barGameObject.transform.localPosition = new Vector3(-size.x / 2f, 0f);

        // Bar Sprite
        GameObject barSpriteGameObject = new GameObject("barSprite", typeof(SpriteRenderer));
        barSpriteGameObject.transform.SetParent(barGameObject.transform);
        barSpriteGameObject.transform.localPosition = new Vector3(size.x / 2f, 0f);
        barSpriteGameObject.transform.localScale = size;
        barSpriteGameObject.GetComponent<SpriteRenderer>().color = barColor;
        barSpriteGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.singlePixelSprite;
        barSpriteGameObject.GetComponent<SpriteRenderer>().sortingOrder = 110;

        HealthBar healthBar = healthBarGameObject.AddComponent<HealthBar>();
        HealthSystem healthSystem = new HealthSystem(maxHealth);
        healthBar.SetUp(healthSystem, offset, fallow, barGameObject.transform, contentGameObject, hideWhenFull);
        return healthSystem;
    }
    
    public static HealthSystem Create(int maxHealth, Vector3 position, Vector3 size, Color barColor, Color backgroundColor, Border border = null, bool hideWhenFull = false)
    {
        return Create(maxHealth, null, position, size, barColor, backgroundColor, border, hideWhenFull);
    }






    private bool hideWhenFull;
    private Vector3 offset;
    private Transform bar;
    private Transform fallow;
    private GameObject contentGameObject;

    private HealthSystem healthSystem;


    private void LateUpdate()
    {
        transform.position = fallow.position + offset;
    }

    private void SetUp(HealthSystem healthSystem, Vector3 offset, Transform fallow, Transform bar, GameObject contentGameObject, bool hideWhenFull)
    {
        this.hideWhenFull = hideWhenFull;
        this.offset = offset;
        this.bar = bar;
        this.fallow = fallow;
        this.contentGameObject = contentGameObject;

        this.healthSystem = healthSystem;

        if (hideWhenFull)
        {
            contentGameObject.SetActive(false);
        }
        
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, HealthSystem.OnHealthChangedEventArgs e)
    {
        if (hideWhenFull)
        {
            bool hide = e.value == healthSystem.GetMaxHealth();
            contentGameObject.SetActive(hide);
        }

        bar.localScale = new Vector3(e.amount, 1);
    }

}