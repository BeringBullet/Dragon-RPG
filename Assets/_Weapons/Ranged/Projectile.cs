using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 10;
        GameObject spwanParent;
        const float DESTROY_DELAY = .01f;
        float damageCaused;

        public float ProjectileSpeed => projectileSpeed;
        public void SetDamage(float damage) => damageCaused = damage;
        public void SetParent(GameObject parent) => spwanParent = parent;

        private void OnCollisionEnter(Collision collision)
        {
            if (spwanParent && collision.gameObject == spwanParent) return;
            Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damagableComponent)
            {
                (damagableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}