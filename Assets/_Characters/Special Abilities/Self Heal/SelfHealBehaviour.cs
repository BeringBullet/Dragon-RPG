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
            PayParticleEffect();
            PlayAudio();
            value.player.Heal(((SelfHealCongif)Config).ExtraHealth);
        }
    }
}