using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI; // TODO consider re-wiring

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        CameraRaycaster cameraRaycaster = null;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.onMouseOverWalkable += CameraRaycaster_onMouseOverWalkable;
            cameraRaycaster.onMouseOverEnemy += CameraRaycaster_onMouseOverEnemy;
        }

        private void CameraRaycaster_onMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                aiCharacterControl.SetTarget(enemy.transform);
            }
        }

        private void CameraRaycaster_onMouseOverWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }
    }
}