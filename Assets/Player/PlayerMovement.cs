using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveRadius = 5;

    ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination;
    Vector3 clickPoint;
    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        character = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) //todo: Allow player to map later or add to menu
        {
            currentDestination = transform.position;
            isInDirectMode = !isInDirectMode;
        }
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);
        
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 move = v * camForward + h * Camera.main.transform.right;

        character.Move(move, crouch,false);

    }
    
    private void ProcessMouseMovement()
    {
        var offset = 0f;
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.CurrentHit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = Destination(walkMoveStopRadius);
                    offset = walkMoveStopRadius;
                    break;
                case Layer.Enemy:
                    currentDestination = Destination(attackMoveRadius);
                    offset = attackMoveRadius;
                    break;
                case Layer.RaycastEndStop:
                    break;
                default:
                    break;
            }
        }
        WalkToDestination(offset);
    }

    private void WalkToDestination(float offset)
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= offset)
        {
            character.Move(playerToClickPoint, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 Destination(float offset)
    {
        Vector3 offsetDestination = (clickPoint - transform.position).normalized * offset;
        return clickPoint - offsetDestination;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, .15f);

        Gizmos.color = new Color(255f,0f,0f,.5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveRadius);
    }
}

