using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbilities
    {
        public AreaEffectConfig Config { set; private get; }
        

        public void Use(AbilityUseParams value)
        {
            PayParticleEffect();
            DealRadialDamage(value);
        }

        private void PayParticleEffect()
        {
            var prefab = Instantiate(Config.ParticleSystemPrefab, transform.position, Quaternion.identity);
            var particleSystem = prefab.GetComponent<ParticleSystem>();
            particleSystem.Play();
            GameObject.Destroy(prefab.gameObject, prefab.main.duration + prefab.main.startLifetime.constantMax);
        }

        private void DealRadialDamage(AbilityUseParams value)
        {
            int layerMask = 1 << gameObject.layer;
            layerMask = ~layerMask;

            Collider[] hits = Physics.OverlapSphere(transform.position, Config.Radius, layerMask);
            foreach (var hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    var amount = Mathf.Clamp(value.baseDamage + Config.Damage, 0, 100);
                    damageable.AdjustHealth(amount);
                }
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            print($"Area Effect behaviour attached to {gameObject.name}");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}