using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters.Weapons
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currectWeaponConfig = null;
        [Range(.1f, 1f)] [SerializeField] float criticalHitChance = 0.1f;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        GameObject target;
        GameObject weaponObject;
        float lastHitTime;

        Animator animator;
        Character character;

        public WeaponConfig CurrectWeaponConfig => currectWeaponConfig;
        float CalculateDamage => baseDamage + currectWeaponConfig.AdditionalDamage;

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeaponInHand(currectWeaponConfig);
            SetAttackAnimation();
        }

        private void SetAttackAnimation()
        {
            if (!character.OverrideController)
            {
                Debug.Break();
                Debug.LogAssertion($"Please provide {gameObject} with an animator override controller");
            }
            else
            {
                var animatorOverrideController = character.OverrideController;
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = currectWeaponConfig.AttackAnimClip;
            }
        }

        internal void StopAttacking()
        {
            StopAllCoroutines();
        }

        // Update is called once per frame
        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;

            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                targetIsDead = target.GetComponent<HealthSystem>().healthAsPercentage <= Mathf.Epsilon;
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currectWeaponConfig.MaxAttackRange;
            }

            var characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIdDead = (characterHealth <= Mathf.Epsilon);

            if (characterIdDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }

        }

        public void PutWeaponInHand(WeaponConfig weaponConfig)
        {
            var weaponPrefab = weaponConfig.WeaponPrefab;
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currectWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currectWeaponConfig.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        public void AttackTarget(GameObject target)
        {
            this.target = target;
            StartCoroutine(AttackTargetRepeatedly);
        }

        IEnumerator AttackTargetRepeatedly
        {
            get
            {
                bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
                bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
                while (attackerStillAlive && targetStillAlive)
                {
                    float weaponHitPeriod = currectWeaponConfig.MinTimeBetweenHits;
                    float timeToWait = weaponHitPeriod * character.AnimationSpeed;
                    bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                    AttackTarget();
                    lastHitTime = Time.time;
                    yield return new WaitForSeconds(timeToWait);
                }
            }
        }

        private void AttackTarget()
        {
            transform.LookAt(target.transform);
            SetAttackAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = 5f; //todo get from the weapon
            StartCoroutine(DamageAfterDelay(damageDelay));

        }
        IEnumerator DamageAfterDelay(float damageDelay)
        {
            yield return new WaitForSecondsRealtime(damageDelay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage);
        }
    }
}