using GameDevTV.Utils;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpFX = null;
        [SerializeField] bool shouldUseModifiers = false;
        public event Action onLevelup;
        Experience experience;

        LazyValue<int> currentLevel;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperiencedGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperiencedGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value && newLevel > startingLevel)
            {
                currentLevel.value = newLevel;
                Instantiate(levelUpFX, this.transform);               
                onLevelup();
            }
            currentLevel.value = newLevel;
        }


        public float GetStat(Stats stat)
        {
            if (shouldUseModifiers)
            {
                return (progression.GetStat(characterClass, stat, GetLevel()) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
            }
            else
            {
                return progression.GetStat(characterClass, stat, GetLevel());
            }
        }

        private float GetPercentageModifier(Stats stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stats stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel()
        {

            int level = 1;

            if (gameObject.GetComponent<Experience>() == null) return startingLevel;

            float  currentXP = GetComponent<Experience>().CheckExp();
            List<float> expLevels = progression.GetExpLevels(characterClass, Stats.ExpToLevelUp);

            for (int i = 0; i < expLevels.Count; i++)
            {
                if (currentXP < expLevels[i]) return level;
                else level++;
            }

            return level;
        }
    }

}
