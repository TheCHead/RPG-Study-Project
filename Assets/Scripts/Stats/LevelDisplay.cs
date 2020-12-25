using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] Text levelDisplayText = null;
        BaseStats playerStats = null;

        private void Awake()
        {
            playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            levelDisplayText.text = playerStats.GetLevel().ToString();
        }
    }
}


