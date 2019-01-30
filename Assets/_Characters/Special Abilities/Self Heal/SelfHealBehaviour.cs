using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbilities
    {
        public SelfHealCongif Config { set; private get; }

        public void Use(AbilityUseParams value)
        {
            Config.PayParticleEffect(transform.position);
            Config.PlayAudio(value.player.AudioSource);
            value.player.Heal(Config.ExtraHealth);
        }
      

        // Start is called before the first frame update
        void Start()
        {
            print($"Self Heal behaviour attached to {gameObject.name}");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}