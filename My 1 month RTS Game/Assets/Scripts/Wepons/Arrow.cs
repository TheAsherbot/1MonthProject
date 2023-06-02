using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    
    [SerializeField] private float timeToHit = 0.5f;


    private int damage = 0;
    private float elapsedTime;
    private Vector3 startPosition;
    private Transform target;


    public void Init(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
        startPosition = transform.position;
    }


    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / timeToHit;

        transform.position = Vector3.Lerp(startPosition, target.position, percentageComplete);
        if (elapsedTime >= timeToHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == target) 
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(damage);
            }
            Destroy(gameObject);
        }
    }

}
