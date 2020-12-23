using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject
    {
        [SerializeField] List<ProgressionCharacterClass> characterClasses = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass charClass;
            public List<float> health = new List<float>();
            public List<float> damage = new List<float>();
        }

        public float GetHealthStat(CharacterClass charClass, int level)
        {
            float output = 50;
            foreach (ProgressionCharacterClass c in characterClasses)
            {
                if (c.charClass ==  charClass)
                {
                    output = c.health[level - 1];
                    return output;
                }
            }
            return output;
        }
    }
}


