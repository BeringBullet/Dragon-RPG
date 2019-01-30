using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams value)
        {
            Config.PayParticleEffect(transform.position);
            Config.PlayAudio(value.player.AudioSource);
            value.player.Heal(((SelfHealCongif)Config).ExtraHealth);
        }
    }
}