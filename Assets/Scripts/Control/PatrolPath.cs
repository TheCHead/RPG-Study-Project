using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i < transform.childCount - 1)
                {
                    Gizmos.color = Color.grey;
                    Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
                }
                else
                {
                    Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(0).position);
                }
            }
        }
    }
}


