using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerMovement player;
        private void Start()
        {
            player = GetComponent<PlayerMovement>();
        }

        public override void Use(GameObject target)
        {
            var playerHealth = player.GetComponent<HealthSystem>();
            PayParticleEffect();
            PlayAudio();
            playerHealth.Heal(((SelfHealCongif)Config).ExtraHealth);
        }
    }
}