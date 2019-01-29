using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Self Heal"))]
    public class SelfHealCongif : SpecialAbility
    {
        [Header("Self Heal specific")]
        [SerializeField] float healthAmount = 10f;
        public float HealthAmount => healthAmount;


        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behavoirComponent = gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();
            behavoirComponent.Config = this;
            behaviour = behavoirComponent;
        }
    }
}
