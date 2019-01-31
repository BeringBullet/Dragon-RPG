﻿using RPG.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        // Update is called once per frame
        void Update()
        {
            DestroyChildron();
            InstantiateWeapon();
        }

        private void DestroyChildron()
        {
            foreach (Transform item in transform)
            {
                if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(transform))
                    UnityEditor.PrefabUtility.UnpackPrefabInstance(gameObject, UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);

                DestroyImmediate(item.gameObject);
            }
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.WeaponPrefab;
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter()
        {
            audioSource.PlayOneShot(pickUpSFX);
            FindObjectOfType<Player>().PutWeaponInHand(weaponConfig);
        }
    }
}