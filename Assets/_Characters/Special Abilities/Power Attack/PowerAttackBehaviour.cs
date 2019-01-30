using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbilities
    {
       public PowerAttackCongif Config { set; private get; }

        public void Use(AbilityUseParams value)
        {
            PayParticleEffect();
            DealDamage(value);
        }

        private void DealDamage(AbilityUseParams value)
        {
            var amount = Mathf.Clamp(value.baseDamage + Config.ExstraDamage, 0, 100);
            value.target.AdjustHealth(amount);
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
            print($"Power Attack behaviour attached to {gameObject.name}");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
