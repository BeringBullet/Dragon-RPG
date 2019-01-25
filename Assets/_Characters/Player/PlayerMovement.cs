using System;
using UnityEngine;
using RPG.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Charactor
{
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        ThirdPersonCharacter character = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        AICharacterControl aiCharacterControl = null;
        NavMeshAgent navMeshAgent = null;
        GameObject walkTarget = null;
        Vector3 currentDestination;
        Vector3 clickPoint;


        [SerializeField] const int uiLayerNumber = 5;
        [SerializeField] const int walkableLayerNumber = 9;
        [SerializeField] const int enemyLayerNumber = 10;


        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            walkTarget = new GameObject("walkTarget");
            currentDestination = transform.position;

            cameraRaycaster.notifyMouseClickObservers += CameraRaycaster_notifyMouseClickObservers;
        }

        private void CameraRaycaster_notifyMouseClickObservers(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case walkableLayerNumber:
                    walkTarget.transform.position = raycastHit.point;
                    aiCharacterControl.SetTarget(walkTarget.transform);
                    break;
                case enemyLayerNumber:
                    GameObject enemy = raycastHit.collider.gameObject;
                    aiCharacterControl.SetTarget(enemy.transform);
                    break;
                default:
                    Debug.LogWarning("Don't know how to handle this click movement");
                    return;
            }

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G)) //todo: Allow player to map later or add to menu
            {
                currentDestination = transform.position;
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.black;
        //    Gizmos.DrawLine(transform.position, currentDestination);
        //    Gizmos.DrawSphere(currentDestination, 0.1f);
        //    Gizmos.DrawSphere(clickPoint, .15f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, attackMoveRadius);
        //}
    }

}