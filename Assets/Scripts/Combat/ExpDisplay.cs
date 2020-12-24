using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ExpDisplay : MonoBehaviour
    {
        [SerializeField] Text expDisplayText = null;

        private void Awake()
        {
            
        }


        private void Update()
        {
            expDisplayText.text = GameObject.FindWithTag("Player").GetComponent<Experience>().CheckExp().ToString();
        }
    }

    
}


