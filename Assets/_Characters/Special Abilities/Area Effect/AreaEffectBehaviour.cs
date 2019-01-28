using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbilities
    {
        public AreaEffectConfig Config { set; private get; }

        public void Use(AbilityUseParams value)
        {
            int layerMask = 1 << gameObject.layer;
            layerMask = ~layerMask;

            print($"Area Effect used, base: {value.baseDamage} extra damage: {Config.Damage}");
            Collider[] hits = Physics.OverlapSphere(transform.position, Config.Radius, layerMask);
            foreach (var hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    var amount = Mathf.Clamp(value.baseDamage + Config.Damage, 0, 100);
                    damageable.TakeDamage(amount);
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