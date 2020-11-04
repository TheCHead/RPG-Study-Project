using RPG.Combat;
using RPG.Core;
using RPG.Movement;
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

        Vector3 guardPosition;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return;  }

            if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
            {
                InteractWithCombat();
            }

            else
            {
                GetComponent<Fighter>().Cancel();
                GetComponent<Mover>().StartMoveAction(guardPosition);
            }

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


