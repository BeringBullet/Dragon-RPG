using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    abstract public class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] ParticleSystem particleSystemPrefab = null;
        public float EnergyCost => energyCost;
        public ParticleSystem ParticleSystemPrefab => particleSystemPrefab;

        protected ISpecialAbilities behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams value) => behaviour.Use(value);
    }

    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public interface ISpecialAbilities
    {
        void Use(AbilityUseParams value);
    }
}
