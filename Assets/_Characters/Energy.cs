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
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointPerSecond = 1f;

        float currentEnergyPoints;
        CameraRaycaster cameraRaycaster;

        float EnergyAsPercent => currentEnergyPoints / maxEnergyPoints;
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
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
            float xValue = -(EnergyAsPercent / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}