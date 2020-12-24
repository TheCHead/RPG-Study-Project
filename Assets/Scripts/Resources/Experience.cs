using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float expPoints = 0;

        public void GetExp(float exp)
        {
            expPoints += exp;
        }



        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            float savedExp = (float)state;
            expPoints = savedExp;
        }

        public float CheckExp()
        {
            return expPoints;
        }
    }
}


