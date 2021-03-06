﻿using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter = null;
        //Health enemyHealth = null;
        [SerializeField] Text enemyHealthDisplayText;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            enemyHealthDisplayText.text = "N/A";
            if (fighter.GetTarget() == null) return;

            enemyHealthDisplayText.text = fighter.GetTarget().GetCurrentHealth().ToString("000.0");
        }
    }
}
