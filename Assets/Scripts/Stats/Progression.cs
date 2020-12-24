using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject
    {
        [SerializeField] List<ProgressionCharacterClass> characterClasses = null;



        public float GetStat(CharacterClass charClass, Stats stat, int level)
        {
            float output = 0;
            foreach (ProgressionCharacterClass c in characterClasses)
            {
                if (c.charClass == charClass)
                {
                    foreach (charClassStats s in c.stats)
                    {
                        if (s.stat == stat)
                        {
                            output = s.levels[level - 1];
                            return output;
                        }
                    }
                }
            }
            return output;
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


