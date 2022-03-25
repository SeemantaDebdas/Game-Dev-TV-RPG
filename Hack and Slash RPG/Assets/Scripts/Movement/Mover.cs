using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction
    {
        NavMeshAgent navMeshAgent;
        Animator anim;
        Health health;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead;

            UpdateAnimation();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }

        void UpdateAnimation()
        {
            anim.SetFloat("VelocityZ", navMeshAgent.velocity.magnitude);
        }

        public void CancelAction()
        {
            //cancel this action
            navMeshAgent.isStopped = true;
        }
    }
}
