using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire...
using RPG.CameraUI;
using RPG.Core;
using UnityEngine.SceneManagement;
using RPG.Characters.Weapons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon currectWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;

        [Range(.1f, 1f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem CriticalHitParticle;
        


        //temperarily serializing for debugging
        [SerializeField] AbilityConfig[] abilities;


        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        Enemy currentEnemy;
        Coroutine deathCR;
        Animator animator;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;
        GameObject weaponObject;

   

        void Start()
        {
            RegisterForMouseClick();
            PutWeaponInHand(currectWeaponConfig);
            SetAttackAnimation();
            AttachInitialAbilities();
        
        }
        private void Update()
        {
            var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercentage > Mathf.Epsilon)
            {
                SnanForAbilityKeyDown();
            }            
        }


        private void AttachInitialAbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null)
                    abilities[i].AttachAbilityTo(gameObject);
            }
        }


        private void SnanForAbilityKeyDown()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                int keyIndex = i + 1;
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(i);
                }
            }
        }

  


        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currectWeaponConfig.AttackAnimClip;
        }

        public void PutWeaponInHand(Weapon weaponConfig)
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

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += CameraRaycaster_onMouseOverEnemy;
        }

        private void CameraRaycaster_onMouseOverEnemy(Enemy enemy)
        {
            currentEnemy = enemy;
            if (Input.GetMouseButton(0) && IsTargetInRange())
            {
                AttackTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0);
            }
        }

        private void AttemptSpecialAbility(int abilityIndes)
        {
            var energyComponent = GetComponent<Energy>();
            var ability = abilities[abilityIndes];
            var energyCost = ability.EnergyCost;

            if (energyComponent.IsEnergyAvalibale(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityPerams = new AbilityUseParams(currentEnemy, baseDamage);
                ability.Use(abilityPerams);
            }
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currectWeaponConfig.MinTimeBetweenHits)
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                //currentEnemy.TakeDamage(CalculateDamage);
                lastHitTime = Time.time;
            }
        }
        
        private float CalculateDamage
        {
            get
            {
                float damage = baseDamage + currectWeaponConfig.AdditionalDamage;
                bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
                if (isCriticalHit)
                {
                    CriticalHitParticle.Play();
                    damage = damage * criticalHitMultiplier;
                }
                return damage;
            }
        }

        private bool IsTargetInRange()
        {
            float distanceToTarget = (currentEnemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= currectWeaponConfig.MaxAttackRange;
        }

        public void TakeDamage(float damage)
        {
            throw new NotImplementedException();
        }
    }
}