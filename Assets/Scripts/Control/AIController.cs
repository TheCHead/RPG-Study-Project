using RPG.Combat;
using RPG.Core;
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

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
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
            }

        }

        private void InteractWithCombat()
        {
            if (GetComponent<Fighter>().CanAttack(player))
            {
                GetComponent<Fighter>().Attack(player);
            }
        }
    }
}


