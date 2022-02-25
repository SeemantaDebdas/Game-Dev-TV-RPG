using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Camera cam;
        Mover mover;
        Fighter fighter;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            cam = Camera.main;
        }

        private void Update()
        {
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;

            print("nothing to do!");
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRayFromMouse());
            foreach (var hit in hits)
            {
                if (!hit.collider.TryGetComponent(out CombatTarget combatTarget)) continue;

                if (!fighter.CanAttack(combatTarget)) continue;

                if (Input.GetMouseButtonDown(0))
                        fighter.Attack(combatTarget);
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

