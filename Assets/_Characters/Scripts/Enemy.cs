using RPG.Core;
using RPG.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters.ThirdPerson;

namespace RPG.Charactor
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 10f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 0.5f;
        [SerializeField] Vector3 verticalAimOffset = new Vector3(0, 1, 0);


        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;


        float currentHealthPoints;

        AICharacterControl aiCharacterControl = null;
        GameObject player = null;

        bool isChassingPlayer = false;
        bool isAttackingPlayer = false;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            isChassingPlayer = distanceToPlayer <= chaseRadius;

            if (distanceToPlayer <= attackRadius && !isAttackingPlayer)
            {
                isAttackingPlayer = true;
                InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots);
                //SpawnProjectile();
            }

            if (distanceToPlayer > attackRadius)
            {
                isAttackingPlayer = false;
                CancelInvoke("SpawnProjectile");
            }

            if (isChassingPlayer)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }

        }

        void SpawnProjectile()
        {

            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectilecomponent = newProjectile.GetComponent<Projectile>();
            projectilecomponent.SetParent(gameObject);
            projectilecomponent.SetDamage(damagePerShot);

            Vector3 unitVectorToPlayer = (player.transform.position + verticalAimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectilecomponent.ProjectileSpeed;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        public float healthAsPercentage => currentHealthPoints / maxHealthPoints;

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);


            if (isChassingPlayer)
            {
                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
        #endregion
    }
}