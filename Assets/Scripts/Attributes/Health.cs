using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] TakeDamageEvent takeDamage;

        LazyValue<float> currentHealth;
        float maxHealth = 0f;
        public bool isDead = false;
        GameObject instigator = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }



        private void Awake()
        {          
            currentHealth = new LazyValue<float>(GetInitialHealth);
        }

        

        public float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelup += UpgradeHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelup -= UpgradeHealth;
        }

        

        private void Start()
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            currentHealth.ForceInit();

            //if (currentHealth < 0)
            //{               
            //    currentHealth = maxHealth;
            //}
        }

        private void UpgradeHealth()
        {
            float nextMaxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            currentHealth.value += (nextMaxHealth - maxHealth);
            maxHealth = nextMaxHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            takeDamage.Invoke(damage);
            this.instigator = instigator;

            currentHealth.value = Mathf.Max(currentHealth.value - damage, 0);
            if (currentHealth.value <= 0)
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
            data.health = currentHealth.value;
            data.isDead = isDead;

            return data;
        }

        public void RestoreState(object state)
        {
            HealthSaveData data = (HealthSaveData)state;
            currentHealth.value = data.health;
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
            return currentHealth.value;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }
    }
}

