using RPG.Resources;
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
        public event Action onLevelup;

        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperiencedGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel && newLevel > startingLevel)
            {
                currentLevel = newLevel;
                Instantiate(levelUpFX, this.transform);               
                onLevelup();
            }
            currentLevel = newLevel;
        }


        public float GetStat(Stats stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
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
