using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float expPoints = 0;

        //public delegate void ExperienceGained();
        public event Action onExperiencedGained;


        public void GetExp(float exp)
        {
            expPoints += exp;
            onExperiencedGained();
        }



        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            expPoints = 0;
            float savedExp = (float)state;
            GetExp(savedExp);

        }

        public float CheckExp()
        {
            return expPoints;
        }
    }
}


