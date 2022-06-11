using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction,ISaveable
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
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

            if(currentWeapon == null)
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
            if (weapon == null)
            {
                Debug.Log($"{name} is trying to equip a null weapon.");
            }
            if (weapon == null) return;  
            currentWeapon = weapon;
            weapon.SpawnWeapon(rightHandTransform,leftHandTransform, anim);
        }

        public object CaptureState()
        {
            if (currentWeapon == null)
            {
                Debug.Log($"{name} does not have a weapon equipped in CaptureState()");
            }
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            //loading the weapon from assets folder
            //that matches the name of the default weapon
            Weapon weapon;
            weapon = Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }


        //Animation Event called from Attack Animation
        void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile)
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.WeaponDamage);
            }
        }

        //Animation Event called from Bow animation
        void Shoot() => Hit();

    }
}


