using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbilities
    {
       public PowerAttackCongif Config { set; private get; }

        public void Use()
        {
            print("Go for PowerAttack!!");
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
