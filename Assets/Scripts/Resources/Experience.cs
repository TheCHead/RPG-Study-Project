using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float expPoints = 0;

        public void GetExp(float exp)
        {
            expPoints += exp;
        }
    }
}


