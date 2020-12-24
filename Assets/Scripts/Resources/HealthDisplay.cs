//using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        [SerializeField] Text healthDisplayText;
        //Health enemyHealth;
        //[SerializeField] Text enemyHealthDisplayText;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            healthDisplayText.text = health.GetCurrentHealth().ToString();

            //enemyHealthDisplayText.text = "N/A";
            //Health targetHealth = health.GetComponent<Fighter>().GetTarget();
            //if (targetHealth != null)
            //{
            //    enemyHealthDisplayText.text = targetHealth.GetCurrentHealth().ToString();
            //}
        }
    }
}
