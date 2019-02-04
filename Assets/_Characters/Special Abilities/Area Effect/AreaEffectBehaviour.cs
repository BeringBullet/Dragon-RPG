
using UnityEngine;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            PlayAnimation();
            PayParticleEffect();
            PlayAudio();
            DealRadialDamage();
        }

    
        private void DealRadialDamage()
        {
            int layerMask = 1 << gameObject.layer;
            layerMask = ~layerMask;

            Collider[] hits = Physics.OverlapSphere(transform.position, ((AreaEffectConfig)Config).Radius, layerMask);
            foreach (var hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<HealthSystem>();
                if (damageable != null)
                {
                    var amount = Mathf.Clamp(((AreaEffectConfig)Config).Damage, 0, 100);
                    damageable.TakeDamage(amount);
                }
            }
        }
    }
}