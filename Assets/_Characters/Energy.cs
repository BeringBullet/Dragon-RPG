using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
using System;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] Image energyOrb;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointPerSecond = 1f;

        float currentEnergyPoints;
        CameraRaycaster cameraRaycaster;

        float EnergyAsPercent => currentEnergyPoints / maxEnergyPoints;
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            energyOrb.fillAmount = EnergyAsPercent;
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

        private IEnumerator AddEnergyPoints()
        {
            var pointsToAdd = regenPointPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);

            UpdateEnergyBar();
            yield return new WaitForSeconds(1);
        }

        public bool IsEnergyAvalibale(float amount) => amount <= currentEnergyPoints;

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            energyOrb.fillAmount = EnergyAsPercent;
        }
    }
}