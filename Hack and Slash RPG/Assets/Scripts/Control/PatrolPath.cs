using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointRadius = 0.15f;
        [SerializeField] Color waypointColor;
        private void OnDrawGizmos()
        {
            Gizmos.color = waypointColor;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWayPoint(i), waypointRadius);
                Gizmos.DrawLine(GetWayPoint(i), transform.GetChild((i + 1) % transform.childCount).position);
            }
        }

        public Vector3 GetWayPoint(int waypointIdx)
        {
            return transform.GetChild(waypointIdx).position;
        }

        public int GetNextIndex(int waypointIdx)
        {
            return (waypointIdx + 1) % transform.childCount;
        }

    }
}

