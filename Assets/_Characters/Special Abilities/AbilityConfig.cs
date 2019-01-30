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
        [SerializeField] ParticleSystem particleSystemPrefab = null;
        [SerializeField] AudioClip audioClip;

        
        public float EnergyCost => energyCost;
        public ParticleSystem ParticleSystemPrefab => particleSystemPrefab;

        protected ISpecialAbilities behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams value) => behaviour.Use(value);

        public void PlayAudio(AudioSource audioSource)
        {
            if (audioClip != null && audioSource != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }


        /// <summary>
        /// Player the Particle for the Ability, used tranform.position
        /// </summary>
        /// <param name="position"></param>
        public void PayParticleEffect(Vector3 position)
        {
            var prefab = Instantiate(ParticleSystemPrefab, position, ParticleSystemPrefab.transform.rotation);
            var particleSystem = prefab.GetComponent<ParticleSystem>();
            particleSystem.Play();
            GameObject.Destroy(prefab.gameObject, prefab.main.duration + prefab.main.startLifetime.constantMax);
        }

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

    public interface ISpecialAbilities
    {
        void Use(AbilityUseParams value);
    }
}
