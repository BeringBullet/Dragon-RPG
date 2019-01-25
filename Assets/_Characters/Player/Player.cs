using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Charactor
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] int enemyLayer = 10;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 10f;

        [SerializeField] Weapon weaponToUse;
        [SerializeField] GameObject weaponSocket;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        Animator animator;
        float lastHitTime;
        float currentHealthPoints;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;

        private void Start()
        {
            RegisterClickEvents();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            setupRuntimeAnimator();

        }

        private void SetCurrentMaxHealth() => currentHealthPoints = maxHealthPoints;

        private void setupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = WeaponToUse.AttackAnimation;
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = WeaponToUse.WeaponPrefab;
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform);
            weapon.transform.localPosition = WeaponToUse.gripTransform.localPosition;
            weapon.transform.localRotation = WeaponToUse.gripTransform.localRotation;
        }

        private void RegisterClickEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        private void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                currentTarget = raycastHit.collider.gameObject;
                if (IsTargerInRange())
                {
                    AttackTarget();
                }
            }
        }
        private void AttackTarget()
        {
            var enemyComponent = currentTarget.GetComponent<IDamageable>();
            if (Time.time - lastHitTime > WeaponToUse.MinTimeBetweenHit)
            {
                animator.SetTrigger("Attack"); //TODO make const
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargerInRange()
        {
            float distanceToTarget = (currentTarget.transform.position - transform.position).magnitude;
            return distanceToTarget <= WeaponToUse.MaxAttackRange;
        }

        public float healthAsPercentage => currentHealthPoints / maxHealthPoints;

        public Weapon WeaponToUse { get => weaponToUse; set => weaponToUse = value; }

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            //if (currentHealthPoints <=0) { Destroy(gameObject); }
        }
    }
}