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
            InteractWithMovement();
            InteractWithCombat();
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButtonDown(0))
                MoveToCursor();
        }

        private void MoveToCursor()
        {
            if (Physics.Raycast(GetRayFromMouse(), out RaycastHit hitInfo))
            {
                mover.MoveTo(hitInfo.point);
            }
        }

        private Ray GetRayFromMouse() 
        {
            return cam.ScreenPointToRay(Input.mousePosition);
        }

        void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRayFromMouse());
            foreach(var hit in hits)
            {
                if(hit.collider.TryGetComponent(out CombatTarget combatTarget))
                {
                    if(Input.GetMouseButtonDown(0))
                        fighter.Attack(combatTarget);
                }
            }
        }
    }
}

