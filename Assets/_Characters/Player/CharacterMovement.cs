using RPG.CameraUI;
using System;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float StoppingDistacne = 1f;
        ThirdPersonCharacter character;

        GameObject walkTarget;
        NavMeshAgent agent;

        void Start()
        {
            character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.stoppingDistance = StoppingDistacne;

            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverWalkable += CameraRaycaster_onMouseOverWalkable;
            cameraRaycaster.onMouseOverEnemy += CameraRaycaster_onMouseOverEnemy;
        }

        private void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity);
            }
            else
            {
                character.Move(Vector3.zero);
            }
        }


        private void CameraRaycaster_onMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
            }
        }

        private void CameraRaycaster_onMouseOverWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }
    }
}