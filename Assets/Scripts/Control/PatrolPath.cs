using UnityEngine;

namespace CCC.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float _waypointGizmoRadius = 0.3f;

        /// <summary>
        /// Callback to draw gizmos that are pickable and always drawn.
        /// </summary>
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.yellow;
                int j = GetNext(i);
                
                Gizmos.DrawSphere(GetWaypoint(i), _waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNext(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
