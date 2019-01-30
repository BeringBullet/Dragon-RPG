using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbilities
    {
        public SelfHealCongif Config { set; private get; }
        Player player;

        public void Use(AbilityUseParams value)
        {
            PayParticleEffect();
            player.AdjustHealth(-Config.ExtraHealth);
        }

        private void PayParticleEffect()
        {
            var prefab = Instantiate(Config.ParticleSystemPrefab, transform.position, Quaternion.identity);
            var particleSystem = prefab.GetComponent<ParticleSystem>();
            particleSystem.Play();
            GameObject.Destroy(prefab.gameObject, prefab.main.duration + prefab.main.startLifetime.constantMax);
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();
            print($"Self Heal behaviour attached to {gameObject.name}");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}