using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;

        public float AdditionalDamage => additionalDamage;
        public float MinTimeBetweenHits => minTimeBetweenHits;
        public float MaxAttackRange => maxAttackRange;
        public GameObject WeaponPrefab => weaponPrefab;

        public AnimationClip AttackAnimClip
        {
            get
            {
                RemoveAnimationEvents();
                return attackAnimation;
            }
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents() => attackAnimation.events = new AnimationEvent[0];
    }
}