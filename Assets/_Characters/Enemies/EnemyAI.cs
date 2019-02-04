using System.Collections;
using UnityEngine;

// TODO consider re-wire
using RPG.Core;
using RPG.Characters.Weapons;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] float waypointDwellTime = 2.0f;
        enum State { idle, patrolling, attacking, chasing, }
        State state = State.idle;

        PlayerControl player = null;
        Character character;
        float currentWeaponRange;
        float distanceToPlayer;
        int nextwaypointIndex;

        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
        }


        void Update()
        {
            var playerHealth = player.GetComponent<HealthSystem>().healthAsPercentage;

            if (playerHealth <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                return;
            }

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.CurrectWeaponConfig.MaxAttackRange;

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(Patrol);
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(ChasePlayer);

            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }
        }

        IEnumerator Patrol
        {
            get
            {
                state = State.patrolling;
                while (patrolPath != null)
                {
                    Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextwaypointIndex).position;
                    character.SetDestination(nextWaypointPos);
                    CycleWaypointWhenClose(nextWaypointPos);
                    yield return new WaitForSeconds(waypointDwellTime);
                }

            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextwaypointIndex = (nextwaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }

        IEnumerator ChasePlayer
        {
            get
            {
                state = State.chasing;
                while (distanceToPlayer >= currentWeaponRange)
                {
                    character.SetDestination(player.transform.position);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}