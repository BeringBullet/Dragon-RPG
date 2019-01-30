using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams value)
        {
            PayParticleEffect();
            PlayAudio();
            DealRadialDamage(value);
        }

    
        private void DealRadialDamage(AbilityUseParams value)
        {
            int layerMask = 1 << gameObject.layer;
            layerMask = ~layerMask;

            Collider[] hits = Physics.OverlapSphere(transform.position, ((AreaEffectConfig)Config).Radius, layerMask);
            foreach (var hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    var amount = Mathf.Clamp(value.baseDamage + ((AreaEffectConfig)Config).Damage, 0, 100);
                    damageable.TakeDamage(amount);
                }
            }
        }
    }
}