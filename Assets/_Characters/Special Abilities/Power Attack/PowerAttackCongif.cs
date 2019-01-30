using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackCongif : AbilityConfig
    {

        [Header("Power Attack specific")]
        [SerializeField] float exstraDamage = 10f;
        public float ExstraDamage => exstraDamage;


        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behavoirComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behavoirComponent.Config = this;
            behaviour = behavoirComponent;
        }
    }
}