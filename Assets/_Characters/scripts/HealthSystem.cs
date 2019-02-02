using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.Characters
{

    [RequireComponent(typeof(CharacterMovement))]
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] deathSound;
        [SerializeField] AudioClip[] damageSound;
        [SerializeField] float deathVanishSeconds = 2f;

        const string DEATH_TRIGGER = "Death";
        Animator animator;

        float currentHealthPoints;
        AudioSource audioSource;
        AudioSource AudioSource => audioSource;

        CharacterMovement characterMovement;


        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();

            currentHealthPoints = maxHealthPoints;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }
        public void Heal(float amount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + amount, 0f, maxHealthPoints);
        }

        public void TakeDamage(float damage)
        {
            bool CharacterDies = (currentHealthPoints - damage <= 0);
           var clip = damageSound[UnityEngine.Random.Range(0, damageSound.Length)];
            audioSource.PlayOneShot(clip);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            if (CharacterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }

        private IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);

            var playerComp = GetComponent<Player>();
            if (playerComp && playerComp.isActiveAndEnabled)
            {
                audioSource.clip = deathSound[UnityEngine.Random.Range(0, deathSound.Length)];
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
                SceneManager.LoadScene(0);
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject, deathVanishSeconds);
            }
        }
    }
}
