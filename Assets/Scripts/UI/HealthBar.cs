using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas healthBarCanvas = null;




        // Update is called once per frame
        void Update()
        {
            float scaleX = health.GetCurrentHealth() / health.GetMaxHealth();
            if (Mathf.Approximately(scaleX, 0) || (Mathf.Approximately(scaleX, 1)))
            {
                healthBarCanvas.enabled = false;
                return;
            }
            healthBarCanvas.enabled = true;
            foreground.localScale = new Vector3(scaleX, 1, 1);
        }
    }
}



