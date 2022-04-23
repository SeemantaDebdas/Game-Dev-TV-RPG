using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
    {
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] float timeBetweenAttacks = 1f;

        ActionScheduler actionScheduler;
        Health target;
        Weapon currentWeapon = null;

        Animator anim;
        readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        readonly int CancelAttackTrigger = Animator.StringToHash("CancelAttackTrigger");

        float timeSinceLastAttack = 0;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead) return;

            transform.LookAt(target.transform);

            if (!(Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.WeaponRange))
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
                anim.ResetTrigger(CancelAttackTrigger);
                anim.SetTrigger(AttackTrigger);
                timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget) 
        {
            target = combatTarget.transform.GetComponent<Health>();
            actionScheduler.StartAction(this);
        }

        public void CancelAction()
        {
            StopAttack();
            GetComponent<Mover>().CancelAction();
            target = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger(AttackTrigger);
            anim.SetTrigger(CancelAttackTrigger);
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.SpawnWeapon(handTransform, anim);
        }


        //Animation Event called from Attack Animation
        void Hit()
        {
            if (target != null)
                target.TakeDamage(currentWeapon.WeaponDamage);
        }
    }
}


