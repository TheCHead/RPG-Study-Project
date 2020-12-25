using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        float health = -1f;
        public bool isDead = false;
        GameObject instigator = null;



        private void Start()
        {
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            this.instigator = instigator;

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
            AwardExperiencePoints();

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<Animator>().ResetTrigger("rise");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperiencePoints()
        {
            if (instigator.GetComponent<Experience>() != null)
            {
                instigator.GetComponent<Experience>().GetExp(gameObject.GetComponent<BaseStats>().GetStat(Stats.Stats.ExperienceReward));
            }
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
            else
            {
                GetComponent<Animator>().SetTrigger("instantDie");
            }

        }

        [System.Serializable]
        struct HealthSaveData
        {
            public float health;
            public bool isDead;
        }

        public float GetCurrentHealth()
        {
            return health;
        }
    }
}

