using UnityEngine;
using CCC.Movement;
using CCC.Combat;
using CCC.Core;

namespace CCC.Control 
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Mover _mover;
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;

        private void Update()
        {
            if(_health.IsDead)
            {
                return;
            }

            if(InteractWithCombat())
            {
                return;
            }

            if(InteractWithMovement())
            {
                return;
            }
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.GetComponent<CombatTarget>();

                if (!_fighter.CanAttack(target?.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

