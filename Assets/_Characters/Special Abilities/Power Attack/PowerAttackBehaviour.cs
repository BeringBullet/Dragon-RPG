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
            print($"Power attack used, base: {value.baseDamage} extra damage: {Config.ExstraDamage}");
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
