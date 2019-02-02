using System.Collections;
using System.Collections.Generic;
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
        float CalculateDamage => baseDamage = baseDamage + currectWeaponConfig.AdditionalDamage;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeaponInHand(currectWeaponConfig); //todo move to weapon system
            SetAttackAnimation();//todo move to weapon system
        }
        private void SetAttackAnimation()
        {
            var animatorOverrideController = character.OverrideController;
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currectWeaponConfig.AttackAnimClip;
        }
        // Update is called once per frame
        void Update()
        {

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
            print($"Attacking {this.target}");
            // todo use a repeat attack co-reoutine
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
    }
}