using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    abstract public class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particleSystemPrefab;
        [SerializeField] AudioClip[] audioClips;

        
        public float EnergyCost => energyCost;
        public GameObject ParticleSystemPrefab => particleSystemPrefab;
        public AudioClip GetRandomAbilitySound
        {
            get
            {
                return audioClips[Random.Range(0, audioClips.Length)];
            }
        }
            
        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviorComponent(GameObject gameObject);
        public void AttachAbilityTo(GameObject gameObject)
        {
            AbilityBehaviour behaviourComponent = GetBehaviorComponent(gameObject);
            behaviourComponent.Config = this;
            behaviour = behaviourComponent;
        }


        public void Use(AbilityUseParams value) => behaviour.Use(value);
    }

    public struct AbilityUseParams
    {
        public Player player;
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage, Player player)
        {
            this.target = target;
            this.baseDamage = baseDamage;
            this.player = player;
        }
    }
}
