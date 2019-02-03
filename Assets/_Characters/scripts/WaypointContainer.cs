using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WaypointContainer : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Vector3 FirstPosition = transform.GetChild(0).position;
            Vector3 prePosition = FirstPosition;
            foreach (Transform waypoint in transform)
            {
                Gizmos.DrawSphere(waypoint.position, .2f);
                Gizmos.DrawLine(prePosition, waypoint.position);
                prePosition = waypoint.position;
            }

            Gizmos.DrawLine(prePosition, FirstPosition);
        }
    }
}
