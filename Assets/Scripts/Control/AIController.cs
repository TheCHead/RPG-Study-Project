using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        GameObject player;
        Health health;

        LazyValue<Vector3> guardPosition;

        [SerializeField] float suspicionTime = 5f;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        [SerializeField] PatrolPath patrolPath;

        int nextWPindex = 0;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        float timeSinceReachedWP = Mathf.Infinity;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedModifier = 0.2f;

        [SerializeField] float stopChaseDistance = 20f;

        bool isTriggered = false;


        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (Vector3.Distance(transform.position, player.transform.position) > stopChaseDistance)
            {
                isTriggered = false;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance || isTriggered)
            {
                isTriggered = true;
                ChaseBehaviour();
            }
            

            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }

            else
            {
                if (patrolPath == null)
                {
                    GuardBehaviour();
                }

                else
                {
                    PatrolBehaviour();
                }
            }

            UpdateTimers();
        }

        public void SetTriggered()
        {
            isTriggered = true;
            print("triggered");
        }


        private void ChaseBehaviour()
        {
            InteractWithCombat();
            timeSinceLastSawPlayer = 0f;
        }

        private void UpdateTimers()
        {
            timeSinceReachedWP += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            if (timeSinceReachedWP < waypointDwellTime)
            {
                return;
            }

            GetComponent<Mover>().StartMoveAction(patrolPath.transform.GetChild(nextWPindex).position, patrolSpeedModifier);

            if (IsAtWaypoint())
            {
                GetNextWaypointIndex();
                timeSinceReachedWP = 0f;
            }

        }

        private bool IsAtWaypoint()
        {
            return Vector3.Distance(transform.position, patrolPath.transform.GetChild(nextWPindex).position) < waypointTolerance;
        }

        private void GetNextWaypointIndex()
        {
            if (nextWPindex == patrolPath.transform.childCount - 1)
            {
                nextWPindex = 0;
            }
            else
            {
                nextWPindex += 1;
            }
        }

        private void GuardBehaviour()
        {
            GetComponent<Mover>().StartMoveAction(guardPosition.value, patrolSpeedModifier);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void InteractWithCombat()
        {
            if (GetComponent<Fighter>().CanAttack(player))
            {
                GetComponent<Fighter>().Attack(player);
            }
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}


