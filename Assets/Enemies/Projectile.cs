using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10;
    GameObject spwanParent;
    float DESTROY_DELAY = 2f;
    float damageCaused;

    public float ProjectileSpeed => projectileSpeed;
    public void SetDamage(float damage) => damageCaused = damage;
    public void SetParent(GameObject parent) => parent = spwanParent;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == spwanParent) return;
        Component damagableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        if (damagableComponent)
        {
            (damagableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, DESTROY_DELAY);
    }
}
