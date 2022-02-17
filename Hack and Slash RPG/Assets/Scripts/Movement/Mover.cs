using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
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

        public void MoveTo(Vector3 destionation)
        {
            navMeshAgent.SetDestination(destionation);
        }

        void UpdateAnimation()
        {
            anim.SetFloat("VelocityZ", navMeshAgent.velocity.magnitude);
        }
    }
}
