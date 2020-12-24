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

        public float GetStat(Stats stat)
        {
            return progression.GetStat(characterClass, stat, startingLevel);
        }

        public float GetExperienceReward()
        {
            return progression.GetStat(characterClass, Stats.ExperienceReward, startingLevel);
        }
    }

}
