using RPG.Resources;
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

        private void Update()
        {
            if (gameObject.tag == "Player")
            {
                print(GetLevel());
            }

        }

        public float GetStat(Stats stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }


        public int GetLevel()
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
