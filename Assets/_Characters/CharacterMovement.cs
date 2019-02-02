using RPG.CameraUI;
using System;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float StoppingDistacne = 1f;
        [SerializeField] float moveSpeedMultiplier = 1.5f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        float turnAmount;
        float forwardAmount;

        Animator animator;
        Rigidbody myRigidbody;
        NavMeshAgent agent;

        void Start()
        {
            animator = GetComponent<Animator>();
            myRigidbody = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();

            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            agent.updatePosition = true;
            agent.updateRotation = false;
            agent.stoppingDistance = StoppingDistacne;

            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverWalkable += CameraRaycaster_onMouseOverWalkable;
            cameraRaycaster.onMouseOverEnemy += CameraRaycaster_onMouseOverEnemy;
        }

        private void Update()
        {
            var move = Vector3.zero;
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                move = agent.desiredVelocity;
            }

            SetForwardAndTurn(move);
            ApplyExtraTurnRotation();
            UpdateAnimator();
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

        private void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                v.y = myRigidbody.velocity.y;
                myRigidbody.velocity = v;
            }
        }

        private void SetForwardAndTurn(Vector3 move)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
            if (move.magnitude > moveThreshold)
            {
                move.Normalize();
            }

            var localMove = transform.InverseTransformDirection(move);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }
}