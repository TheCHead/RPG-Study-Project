using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed = 5f;
        GameObject enemyTarget = null;
        float damage;
        [SerializeField] float projectileDamageModifier = 1f;
        [SerializeField] float projectileLifetime = 8f;

        private void Start()
        {
            DestroyOnTime();
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(GetAimLocation());
            transform.position = Vector3.MoveTowards(transform.position, GetAimLocation(), Time.deltaTime * projectileSpeed);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = enemyTarget.GetComponent<CapsuleCollider>();
            if (targetCollider != null)
            {
                return enemyTarget.transform.position + Vector3.up * (targetCollider.height / 2);
            }
            return enemyTarget.transform.position;
        }

        public void SetEnemyTarget(GameObject enemy, float damageToDeal)
        {
            this.enemyTarget = enemy;
            this.damage = damageToDeal * projectileDamageModifier;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == enemyTarget.gameObject)
            {
                other.gameObject.GetComponent<Health>().TakeDamage(damage);
                Destroy(gameObject);
                other.GetComponent<AIController>().SetTriggered();
            }
        }

        private void DestroyOnTime()
        {
            Destroy(gameObject, projectileLifetime);
        }

    }
}


