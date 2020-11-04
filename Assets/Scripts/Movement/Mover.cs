using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        Ray lastRay;
        NavMeshAgent myNavMeshAvent;
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
            myNavMeshAvent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            myNavMeshAvent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            myNavMeshAvent.destination = destination;
            myNavMeshAvent.isStopped = false;
        }

        public void StopMoving()
        {
            myNavMeshAvent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            GetComponent<Animator>().SetFloat("forwardSpeed", localVelocity.z);
        }

        public void Cancel()
        {
            print("Cancel movement");
        }
    }
}


