using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire...
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [Header("Player Death")] [SerializeField] AudioClip[] deathSound;
        [SerializeField] AudioClip[] damageSound;
        AudioSource audioSource;


        //temperarily serializing for debugging
        [SerializeField] SpecialAbility[] abilities;

        const string DEATH_TRIGGER = "Death";
        const string ATTACK_TRIGGER = "Attack";

        Enemy currentEnemy;
        Coroutine deathCR;
        Animator animator;
        float currentHealthPoints;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AttachInitialAbilities();
        

            audioSource = GetComponent<AudioSource>();
        }

        private void AttachInitialAbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null)
                    abilities[i].AttachComponentTo(gameObject);
            }
        }

        private void Update()
        {
            if (healthAsPercentage > Mathf.Epsilon)
            {
                SnanForAbilityKeyDown();
            }
        }

        private void SnanForAbilityKeyDown()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                int keyIndex = i + 1;
                if (Input.GetKeyDown(keyIndex))
                {
                    AttemptSpecialAbility(i);
                }
            }
        }

        public void AdjustHealth(float value)
        {
            ReduceHealth(value);
            if (currentHealthPoints <= 0)
            {
                StartCoroutine(KillPlayer());            
            }
        }
       
        private IEnumerator KillPlayer()
        {
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSound[UnityEngine.Random.Range(0, deathSound.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            SceneManager.LoadScene(0);
        }

        private void ReduceHealth(float damage)
        {
            audioSource.clip = damageSound[UnityEngine.Random.Range(0, damageSound.Length)];
            audioSource.Play();
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // remove const
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
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
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger(ATTACK_TRIGGER); // TODO make const
                currentEnemy.AdjustHealth(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange()
        {
            float distanceToTarget = (currentEnemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }
    }
}