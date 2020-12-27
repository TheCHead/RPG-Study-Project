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
        float currentHealth = -1f;
        float maxHealth = 0f;
        public bool isDead = false;
        GameObject instigator = null;



        private void Start()
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);

            if (currentHealth < 0)
            {               
                currentHealth = maxHealth;
            }

            BaseStats stats = GetComponent<BaseStats>();
            if (stats != null)
            {
                stats.onLevelup += UpgradeHealth;
            }
        }

        private void UpgradeHealth()
        {
            float nextMaxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            currentHealth += (nextMaxHealth - maxHealth);
            maxHealth = nextMaxHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            this.instigator = instigator;

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            if (currentHealth <= 0)
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
            data.health = currentHealth;
            data.isDead = isDead;

            return data;
        }

        public void RestoreState(object state)
        {
            HealthSaveData data = (HealthSaveData)state;
            currentHealth = data.health;
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
            return currentHealth;
        }
    }
}

