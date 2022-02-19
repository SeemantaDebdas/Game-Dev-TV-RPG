using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
    {
        [SerializeField] float weaponRange = 2f;
        Transform target;

        private void Update()
        {
            if (target == null) return;

            if (!(Vector3.Distance(transform.position, target.position) <= weaponRange))
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().CancelAction();
            }
        }

        public void Attack(CombatTarget combatTarget) 
        {
            target = combatTarget.transform;
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void CancelAttack() => target = null;

        public void CancelAction()
        {
            //cancel this action
            CancelAttack();
        }
    }
}


