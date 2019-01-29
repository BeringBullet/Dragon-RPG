using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbilities
    {
        public SelfHealCongif Config { set; private get; }

        public void Use(AbilityUseParams value)
        {
            int layerMask = 1 << gameObject.layer;
            layerMask = ~layerMask;
            print($"Self Heal used, base: {value.baseDamage} ");
           
        }


        // Start is called before the first frame update
        void Start()
        {
            print($"Self Heal behaviour attached to {gameObject.name}");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}