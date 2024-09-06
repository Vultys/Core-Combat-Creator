using UnityEngine;
using CCC.Movement;
using CCC.Combat;
using CCC.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace CCC.Control
{
    public class PlayerController : MonoBehaviour
    {

        [Serializable]
        struct CursorMapping
        {
            public CursorType type;

            public Texture2D texture;

            public Vector2 hotspot;
        }

        [Header("Settings")]
        [SerializeField] private float _maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float _raycastRadius = 1f;


        [Header("Components")]
        [SerializeField] private Mover _mover;
        [SerializeField] private Health _health;
        [SerializeField] private CursorMapping[] _cursorMappings = null;

        private void Update()
        {
            if (InteractWithUI())
            {
                SetCursor(CursorType.UI);
                return;
            }

            if (_health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithMovement()
        {
            Vector3 target;

            if (RaycastNavMesh(out target))
            {
                if(!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (var mapping in _cursorMappings)
            {
                if (mapping.type == cursorType)
                {
                    return mapping;
                }
            }

            return _cursorMappings[0];
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                var raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.SphereCastAll(GetMouseRay(), _raycastRadius);

            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;

            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false; 
            
            target = navMeshHit.position;

            return true;
        }
    } 
}

