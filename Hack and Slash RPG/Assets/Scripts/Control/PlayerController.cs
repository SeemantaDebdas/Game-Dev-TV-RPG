using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Camera cam;
        Mover mover;
        Fighter fighter;
        Health health;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            cam = Camera.main;
        }

        private void Update()
        {
            if (health.IsDead) return;
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRayFromMouse());
            foreach (var hit in hits)
            {
                if (!hit.collider.TryGetComponent(out CombatTarget combatTarget)) continue;

                if (!fighter.CanAttack(combatTarget.gameObject)) continue;

                if (Input.GetMouseButton(0))
                        fighter.Attack(combatTarget.gameObject);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            if (Physics.Raycast(GetRayFromMouse(), out RaycastHit hitInfo))
            {
                if(Input.GetMouseButton(0))
                    mover.StartMoveAction(hitInfo.point);
                return true;
            }
            return false;
        }

        private Ray GetRayFromMouse() 
        {
            return cam.ScreenPointToRay(Input.mousePosition);
        }

    }
}

