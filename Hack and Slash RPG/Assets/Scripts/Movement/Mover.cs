using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction,ISaveable
    {
        [SerializeField] float maxSpeed = 5f;
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

        public void StartMoveAction(Vector3 destination, float speedFraction = 1)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction = 1)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.speed = maxSpeed * speedFraction;
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

        public object CaptureState()
        {
            return new SerializableVector(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector serializedVector = (SerializableVector)state;
            transform.position = serializedVector.ToVector();
            #region Why Cancel Current Action?
            /*------------------------------------------------
             * Cancelling current action i.e moving or fighting 
             * so that when restoring state, it doesn't cause
             * unwanted behaviour like moving to a spot or 
             * attacking without any enemy in sight
             *------------------------------------------------
             */
            #endregion
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
