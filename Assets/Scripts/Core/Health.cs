using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 20f;
        public bool isDead = false;




        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health <= 0)
            {
                if (!isDead)
                {
                    DeathSequence();
                }
            }

            
        }

        private void DeathSequence()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<Animator>().ResetTrigger("rise");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }


        public object CaptureState()
        {
            HealthSaveData data = new HealthSaveData();
            data.health = health;
            data.isDead = isDead;

            return data;
        }

        public void RestoreState(object state)
        {
            HealthSaveData data = (HealthSaveData)state;
            health = data.health;
            isDead = data.isDead;

            if (!isDead)
            {
                GetComponent<Animator>().SetTrigger("rise");
            }

        }

        [System.Serializable]
        struct HealthSaveData
        {
            public float health;
            public bool isDead;
        }
    }
}

