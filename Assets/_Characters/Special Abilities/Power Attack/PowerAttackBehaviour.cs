using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
       public PowerAttackCongif Config { set; private get; }

        public override void  Use(AbilityUseParams value)
        {
            Config.PayParticleEffect(transform.position);
            Config.PlayAudio(value.player.AudioSource);
            DealDamage(value);
        }

        private void DealDamage(AbilityUseParams value)
        {
            var amount = Mathf.Clamp(value.baseDamage + Config.ExstraDamage, 0, 100);
            value.target.TakeDamage(amount);
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
