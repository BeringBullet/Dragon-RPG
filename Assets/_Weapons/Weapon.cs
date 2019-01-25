using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHit = .5f;
        [SerializeField] float maxAttackRange = 2f;

        public float MinTimeBetweenHit => minTimeBetweenHit;
        public float MaxAttackRange => maxAttackRange;


        public GameObject WeaponPrefab => weaponPrefab;
        public AnimationClip AttackAnimation
        {
            get
            {
                RemvoeAnimationEvents();
                return attackAnimation;
            }
        }

        // So that asset packs cannot cause crashes
        private void RemvoeAnimationEvents() => attackAnimation.events = new AnimationEvent[0];
    }
}