using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        public AbilityConfig Config { set; protected get; }
        const float PARTICLE_CLEAN_UP_DELAY = 20f;
       
        abstract public void Use(GameObject target = null);

        protected void PayParticleEffect()
        {
            var ParticleObject = Instantiate(Config.ParticleSystemPrefab, transform.position, Config.ParticleSystemPrefab.transform.rotation);

            ParticleObject.transform.parent = transform;
            ParticleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyPartice(ParticleObject));
        }

        protected void PlayAudio()
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(Config.GetRandomAbilitySound);
        }

        IEnumerator DestroyPartice(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }
    }
}
