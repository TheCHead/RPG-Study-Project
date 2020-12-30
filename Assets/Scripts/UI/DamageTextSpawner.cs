using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageText = null;


        public void SpawnDamageText(float damage)
        {
            DamageText instance = Instantiate<DamageText>(damageText, transform);
            instance.SetDamageText(damage);
        }
    }
}


