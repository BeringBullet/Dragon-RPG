using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
using System;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointPerSecond = 3f;
        [SerializeField] AudioClip outOfEnergySFX;

        float currentEnergyPoints;
        AudioSource audioSource;

        float EnergyAsPercent => currentEnergyPoints / maxEnergyPoints;
        public int Length => abilities.Length;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;

            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        private void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                StartCoroutine(AddEnergyPoints());
            }
        }

        private void AttachInitialAbilities()
        {
            for (int i = 1; i < abilities.Length; i++) //setting array slot 0 for right click
            {
                if (abilities[i] != null)
                    abilities[i].AttachAbilityTo(gameObject);
            }
        }
        public void AttemptSpecialAbility(int abilityIndes, GameObject target = null)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var ability = abilities[abilityIndes];
            var energyCost = ability.EnergyCost;

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndes].Use(target);
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(outOfEnergySFX);
                }
            }

        }

        private IEnumerator AddEnergyPoints()
        {
            var pointsToAdd = regenPointPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);

            UpdateEnergyBar();
            yield return new WaitForSeconds(regenPointPerSecond);
        }

        //public bool IsEnergyAvalibale(float amount) => amount <= currentEnergyPoints;

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            energyBar.fillAmount = EnergyAsPercent;
        }
    }
}