using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Combat;
using UnityEditor;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance;

        Fighter fighter;
        GameObject player;
        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
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
                fighter.Attack(player);
            }
            else
            {
                fighter.CancelAction();
            }
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