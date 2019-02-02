using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
      
        public override void Use(GameObject target)
        {
            PayParticleEffect();
            PlayAudio();
            DealDamage(target);
        }

        private void DealDamage(GameObject target)
        {
            var amount = Mathf.Clamp(((PowerAttackCongif)Config).ExstraDamage, 0, 100);
            target.GetComponent<HealthSystem>().TakeDamage(amount);
        }
    }
}
