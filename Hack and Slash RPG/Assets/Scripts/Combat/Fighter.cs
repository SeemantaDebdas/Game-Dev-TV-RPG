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
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float timeBetweenAttacks = 1f;

        Transform target;

        Animator anim;
        int AttackTrigger = Animator.StringToHash("AttackTrigger");

        float timeSinceLastAttack = 0;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!(Vector3.Distance(transform.position, target.position) <= weaponRange))
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().CancelAction();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                anim.SetTrigger(AttackTrigger);
                timeSinceLastAttack = 0;
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
            CancelAttack();
        }

        //Animation Event
        void Hit()
        {
            if (target != null)
                target.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }
}


