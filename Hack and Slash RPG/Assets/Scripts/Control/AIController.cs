using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Combat;
using RPG.Movement;
using UnityEditor;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance;
        [SerializeField] float suspiscionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointDistanceTolerance = 0.5f;
        [SerializeField] float patrolWaitingTime = 3f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;
        ActionScheduler actionScheduler;

        Vector3 initialGuardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastWaypoint = Mathf.Infinity;
        private int currentWaypoint = 0;

        private void Awake()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();

            initialGuardPosition = transform.position;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (health.IsDead) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            { 
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspiscionTime)
            {
                SuspiscionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = initialGuardPosition;
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceLastWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceLastWaypoint > patrolWaitingTime)
                mover.StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWaypoint);
        }

        private void CycleWaypoint()
        {
            currentWaypoint = patrolPath.GetNextIndex(currentWaypoint);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint <= waypointDistanceTolerance;
        }

        private void SuspiscionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= chaseDistance;
        }

        private void OnDrawGizmos()
        {
            Handles.color = new Color(0.1f, 0.5f, 1f, 0.1f);
            Handles.DrawSolidDisc(transform.position, Vector3.up, chaseDistance);
        }
    }
}