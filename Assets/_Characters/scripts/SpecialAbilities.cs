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
        //todo: out of energy sound

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
        Coroutine cr;
        private void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                cr = StartCoroutine(AddEnergyPoints());
            }
            else
            {
                if (cr != null)
                {
                    StopCoroutine(cr);
                    cr = null;
                }
            }
        }

        private void AttachInitialAbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null)
                    abilities[i].AttachAbilityTo(gameObject);
            }
        }
        public void AttemptSpecialAbility(int abilityIndes)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var ability = abilities[abilityIndes];
            var energyCost = ability.EnergyCost;

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                print($"Using special ability {abilityIndes}");
            }
            else
            {
                //todo play out of energy sound
            }

        }

        private IEnumerator AddEnergyPoints()
        {
            var pointsToAdd = regenPointPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);

            UpdateEnergyBar();
            yield return new WaitForSeconds(1);
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