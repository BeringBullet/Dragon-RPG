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

        public override AbilityBehaviour GetBehaviorComponent(GameObject gameObject) => gameObject.AddComponent<AreaEffectBehaviour>();

    }
}
