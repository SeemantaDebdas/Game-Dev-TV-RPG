using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction
    {
        NavMeshAgent navMeshAgent;
        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateAnimation();
        }

        public void StartMoveAction(Vector3 destionation)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destionation);
        }

        public void MoveTo(Vector3 destionation)
        {
            navMeshAgent.SetDestination(destionation);
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
