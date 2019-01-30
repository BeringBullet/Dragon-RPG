using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        public AbilityConfig Config { set; protected get; }
        const float PARTICE_CLEAN_UP_DELAY = 20f;

        abstract public void Use(AbilityUseParams value);

        protected void PayParticleEffect()
        {
            var ParticleObject = Instantiate(Config.ParticleSystemPrefab, transform.position, Config.ParticleSystemPrefab.transform.rotation);

            ParticleObject.transform.parent = transform;
            ParticleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyPartice(ParticleObject));
        }

        IEnumerator DestroyPartice(ParticleSystem particlePrefab)
        {
            while (particlePrefab.isPlaying)
            {
                yield return new WaitForSeconds(PARTICE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }
    }
}
