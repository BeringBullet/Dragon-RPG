using RPG.CameraUI;
using System;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Audio")]
        [Range(0, 1f)] [SerializeField] float spatialBlend = .5f;
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar charaterAvatar;
        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadios = .2f;
        [SerializeField] float colliderHeight = 2.3f;
        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = 1.5f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;
        [Header("Nav Mesh Agent")]
        [SerializeField] float SteeringSpeed = 1f;
        [SerializeField] float StoppingDistacne = 1.3f;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody myRigidbody;

        float turnAmount;
        float forwardAmount;
        bool isAlive = true;

        public AnimatorOverrideController OverrideController => animatorOverrideController;
        public float AnimationSpeed => animator.speed;
        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;
            navMeshAgent.autoBraking = false;
            navMeshAgent.stoppingDistance = StoppingDistacne;
            navMeshAgent.speed = SteeringSpeed;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = charaterAvatar;

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadios;
            capsuleCollider.height = colliderHeight;

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = spatialBlend;

        }

        private void Update()
        {
            var move = Vector3.zero;
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                move = navMeshAgent.desiredVelocity;
            }

            Move(move);
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

        public void Kill() => isAlive = false;

        public void SetDestination(Vector3 worldpos) => navMeshAgent.destination = worldpos;

        void Move(Vector3 move)
        {
            SetForwardAndTurn(move);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }


        void SetForwardAndTurn(Vector3 move)
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