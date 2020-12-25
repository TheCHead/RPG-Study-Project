using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject
    {
        [SerializeField] List<ProgressionCharacterClass> characterClasses = null;


        Dictionary<CharacterClass, Dictionary<Stats, List<float>>> lookupTable;


        public float GetStat(CharacterClass charClass, Stats stat, int level)
        {
            BuildLookup();

            float output = 0;

            List<float> levels = lookupTable[charClass][stat];

            if (levels.Count < level) return 0;

            return levels[level - 1];

            //foreach (ProgressionCharacterClass c in characterClasses)
            //{
            //    if (c.charClass == charClass)
            //    {
            //        foreach (charClassStats s in c.stats)
            //        {
            //            if (s.stat == stat)
            //            {
            //                if (s.levels.Count < level) continue;
 
            //                output = s.levels[level - 1];
            //                return output;
            //            }
            //        }
            //    }
            //}
            //return output;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, List<float>>>();

            foreach (ProgressionCharacterClass c in characterClasses)
            {
                Dictionary<Stats, List<float>> statsDict = new Dictionary<Stats, List<float>>();

                foreach (charClassStats s in c.stats)
                {
                    statsDict.Add(s.stat, s.levels);                   
                }
                lookupTable.Add(c.charClass, statsDict);
            }
        }

        public List<float> GetExpLevels(CharacterClass charClass, Stats stat)
        {
            return lookupTable[charClass][stat];
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass charClass;

            public List<charClassStats> stats = null;

            //public List<float> health = new List<float>();
            //public List<float> damage = new List<float>();

        }

        [System.Serializable]
        class charClassStats
        {
            public Stats stat;
            public List<float> levels = null;
        }
    }
}


