using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int enemyLayer = 10;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetwwnHit = .5f;
    [SerializeField] float maxAttackRange = 2f;
    [SerializeField] Weapon weaponToUse;
    [SerializeField] GameObject weaponSocket;


    float lastHitTime;
    float currentHealthPoints;
    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;

    private void Start()
    {
        RegisterClickEvents();
        currentHealthPoints = maxHealthPoints;
        PutWeaponInHand();
    }

    private void PutWeaponInHand()
    {
        var weaponPrefab = weaponToUse.WeaponPrefab;
        var weapon = Instantiate(weaponPrefab, weaponSocket.transform);
        weapon.transform.localPosition = weaponToUse.gripTransform.localPosition;
        weapon.transform.localRotation = weaponToUse.gripTransform.localRotation;
    }

    private void RegisterClickEvents()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    private void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;
            if ((enemy.transform.position - transform.position).magnitude > maxAttackRange) return;

            currentTarget = enemy;
            var enemyComponent = enemy.GetComponent<IDamageable>();
            if (Time.time - lastHitTime > minTimeBetwwnHit)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }
    }

    public float healthAsPercentage => currentHealthPoints / maxHealthPoints;

    void IDamageable.TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        //if (currentHealthPoints <=0) { Destroy(gameObject); }
    }
}
