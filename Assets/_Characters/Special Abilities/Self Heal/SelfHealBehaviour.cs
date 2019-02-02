using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        Player player;
        private void Start()
        {
            player = GetComponent<Player>();
        }

        public override void Use(AbilityUseParams value)
        {
            var playerHealth = player.GetComponent<HealthSystem>();
            PayParticleEffect();
            PlayAudio();
            playerHealth.Heal(((SelfHealCongif)Config).ExtraHealth);
        }
    }
}