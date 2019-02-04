
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerControl player;
        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        public override void Use(GameObject target)
        {
            PlayAnimation();
            var playerHealth = player.GetComponent<HealthSystem>();
            PayParticleEffect();
            PlayAudio();
            playerHealth.Heal(((SelfHealCongif)Config).ExtraHealth);
        }
    }
}