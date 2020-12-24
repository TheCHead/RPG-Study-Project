﻿using System.Collections;
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

        public float GetBaseHealth()
        {
            return progression.GetStat(characterClass, Stats.Health, startingLevel);
        }

        public float GetExperienceReward()
        {
            return 10f;
        }
    }

}
