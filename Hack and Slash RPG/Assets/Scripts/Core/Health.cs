using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        Animator anim;
        ActionScheduler actionScheduler;
        readonly int DeathTrigger = Animator.StringToHash("DeathTrigger");

        public bool IsDead { get { return isDead; } }
        bool isDead = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            anim.SetTrigger(DeathTrigger);
            actionScheduler.CancelCurrentAction();
        }
    }
}

