using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 20f;
        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                if (!isDead)
                {
                    DeathSequence();
                }
            }
        }

        private void DeathSequence()
        {
            Debug.Log("Enemy: Oh no I'm so dead x_x");
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }
    }
}

