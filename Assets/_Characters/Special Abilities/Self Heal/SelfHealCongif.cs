using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Self Heal"))]
    public class SelfHealCongif : AbilityConfig
    {
        [Header("Self Heal specific")]
        [SerializeField] float extraHealth = 10f;
        public float ExtraHealth => extraHealth;

        public override AbilityBehaviour GetBehaviorComponent(GameObject gameObject) => gameObject.AddComponent<SelfHealBehaviour>();
    }
}
