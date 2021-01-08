using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        Ray lastRay;
        NavMeshAgent myNavMeshAvent;
        Health health;
        [SerializeField] float maxSpeed = 5f;
        [SerializeField] float maxNavPathLenght = 40f;

        private void Awake()
        {
            health = GetComponent<Health>();
            myNavMeshAvent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            myNavMeshAvent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination , float speedModifier)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedModifier);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLenght(path) > maxNavPathLenght) return false;

            return true;
        }

        private float GetPathLenght(NavMeshPath path)
        {
            float pathDistance = 0f;
            Vector3[] corners = path.corners;
            pathDistance = Vector3.Distance(transform.position, corners[0]);
            for (int i = 1; i < corners.Length; i++)
            {
                pathDistance += Vector3.Distance(corners[i], corners[i - 1]);
            }
            return pathDistance;
        }

        public void MoveTo(Vector3 destination, float speedModifier)
        {
            myNavMeshAvent.speed = maxSpeed * Mathf.Clamp01(speedModifier);
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
            StopMoving();
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}


