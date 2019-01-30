using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
      
        public override void  Use(AbilityUseParams value)
        {
            Config.PayParticleEffect(transform.position);
            Config.PlayAudio(value.player.AudioSource);
            DealDamage(value);
        }

        private void DealDamage(AbilityUseParams value)
        {
            var amount = Mathf.Clamp(value.baseDamage + ((PowerAttackCongif)Config).ExstraDamage, 0, 100);
            value.target.TakeDamage(amount);
        }
    }
}
