using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect specific")]
        [SerializeField] float damage = 15f;
        [SerializeField] float radius = 5f;
        public float Damage => damage;
        public float Radius => radius;


        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behavoirComponent = gameObjectToAttachTo.AddComponent<AreaEffectBehaviour>();
            behavoirComponent.Config = this;
            behaviour = behavoirComponent;
        }
    }
}
