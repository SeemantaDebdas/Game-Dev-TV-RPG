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

        Health target;

        Animator anim;
        readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        readonly int CancelAttackTrigger = Animator.StringToHash("CancelAttackTrigger");

        float timeSinceLastAttack = 0;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead) return;

            transform.LookAt(target.transform);

            if (!(Vector3.Distance(transform.position, target.transform.position) <= weaponRange))
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
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

        public bool CanAttack(CombatTarget combatTarget)
        {
            return !combatTarget.GetComponent<Health>().IsDead && combatTarget != null;
        }

        public void Attack(CombatTarget combatTarget) 
        {
            target = combatTarget.transform.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void CancelAction()
        {
            anim.SetTrigger(CancelAttackTrigger);
            target = null;
        }


        //Animation Event called from Attack Animation
        void Hit()
        {
            if (target != null)
                target.TakeDamage(weaponDamage);
        }
    }
}


