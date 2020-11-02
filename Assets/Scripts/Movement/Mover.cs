using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] Transform target;
        Ray lastRay;
        NavMeshAgent myNavMeshAvent;

        private void Start()
        {
            myNavMeshAvent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Fighter>().DropTarget();
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

    }
}


