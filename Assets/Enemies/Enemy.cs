using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attachRaduis = 20f;
    float currentHealthPoints = 100f;

    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    bool isChassingPlayer = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        isChassingPlayer = distanceToPlayer <= attachRaduis;
        if (isChassingPlayer)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }

    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attachRaduis);
        if (isChassingPlayer)
        {
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}
